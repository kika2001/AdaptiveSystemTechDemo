using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdaptiveS.System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace AdaptiveSystemDemo.Weapon
{
    public class Weapon : MonoBehaviour
{
    //Tracking Data
    private int shotsFired;
    private int shotsHit;
    //Tracking Data
    private InputMaster controls;
    [Header("Bullet Holes Pool")]
    [SerializeField] private ObjectPool bulletHoleObjectPool;

    [Header("Weapon Stats")]
    [SerializeField] private float fireRate;
    [SerializeField] private int maxBullets;
    [SerializeField] private int currentBullets;
    [SerializeField] private int damage;
    [Range(0,100)]
    [SerializeField] private int critChance;
    [SerializeField] private float critMultiplier;
    [SerializeField] private LayerMask shootLayer;
    private float reloadTime;
    private bool canShoot;
    private bool shooting;
    
    [Header("Visual Stuff")]
    [SerializeField] private ParticleSystem muzzleFlashFx;
    [SerializeField] private ParticleSystem muzzleExplosionFx;
    [SerializeField] private AudioSource weaponAudio;
    [SerializeField] private AudioClip shootSound, emptySound, reloadSound;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private FlickLight flickLight;
    private RaycastHit aimHit;
    
    public WeaponStatus weaponStatus;

    public delegate void dgBullets(WeaponStatus stat,int current, int max);
    public static event dgBullets evBullets;
    private AdaptiveSystem adaptiveSystem;
    
    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Fire.Enable();
        controls.Player.Fire.started += StartedShooting;
        controls.Player.Fire.canceled += StopedShooting;
        controls.Player.Reload.Enable();
        controls.Player.Reload.performed += ReloadWeapon;

        SubscribeAdaptiveSystem();
    }

    private void SubscribeAdaptiveSystem()
    {
        adaptiveSystem = AdaptiveSystemManager.NewAdaptiveSystem("player");
        AdaptiveSystemManager.AddDataToAnalyse("shots",shotsHit,shotsFired,adaptiveSystem);
        //AdaptiveSystemManager.AddDataToAnalyse("shots",shotsHit,shotsFired);
    }
    void Start()
    {
        ResetValues();
    }

    private void Update()
    {
        Shooting();
    }

    private void Shooting()
    {
        if (shooting)
        {
            //Debug.Log("Shooting");
            if (canShoot)
            {
                if (currentBullets>=1)
                {
                    shotsFired++;
                    muzzleFlashFx.Play();
                    muzzleExplosionFx.Play();
                    weaponAudio.PlayOneShot(shootSound,0.7f);
                    weaponStatus = WeaponStatus.Shooting;
                    
                    
                    //weaponAnimator.Play("Shoot");
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out aimHit, Mathf.Infinity,shootLayer))
                    {
                        HandleHit(aimHit);
                        
                    }
                    else
                    {
                        StartCoroutine(FireRate());
                        currentBullets--;
                    } 
                } else
                {
                    //Debug.Log("Out of Bullets. Reloading!");
                    if (!weaponAudio.isPlaying)
                    {
                        weaponAudio.PlayOneShot(emptySound,0.7f);
                    }
                    
                }
                AdaptiveSystemManager.UpdateInfo("shots",shotsHit,shotsFired,adaptiveSystem);
                evBullets?.Invoke(weaponStatus,currentBullets,maxBullets);
                
            }
        }
        else
        {
            //Debug.Log("Not Shooting");
        }
    }
    private void HandleHit(RaycastHit hit)
    {
        //Debug.Log("Shoot Something");
        StartCoroutine(FireRate());
        var crit = (Random.Range(0, 101) < critChance);
        var tempDamage = (crit)? Convert.ToInt32( damage*critMultiplier): damage;
        //Debug.LogWarning($"TempDamage : {tempDamage}");
        if (aimHit.transform.GetComponent<Destructible>())
        {
            //Debug.Log("Destructible Object");
            aimHit.transform.GetComponent<ITakeDamage>().TakeDamage(tempDamage,hit);
            SpawnBulletHole();
        }else if (aimHit.transform.GetComponents<MonoBehaviour>().Count(d => d is ITakeDamage)>0)
        {
            //Debug.Log("HealthSystem Object");
            aimHit.transform.GetComponent<ITakeDamage>().TakeDamage(tempDamage,hit);
            shotsHit++;
        }
        else
        {
            //Debug.Log("Chao");
            SpawnBulletHole();
        }
        currentBullets--;
       
        
        
    }
    private void ReloadWeapon(InputAction.CallbackContext obj)
    {
        if (canShoot)
        {
            StartCoroutine(Reload());
        }
    }

    private void StartedShooting(InputAction.CallbackContext obj)
    {
        
        shooting = true;
    }

    private void StopedShooting(InputAction.CallbackContext obj)
    {
        shooting = false;
        if (weaponStatus!= WeaponStatus.Reloading)
        {
            weaponStatus = WeaponStatus.Idle;
            evBullets?.Invoke(weaponStatus,currentBullets,maxBullets);
        }
        
    }

    private void FireWeapon()
    {
        shotsFired++;
    }
    
    private IEnumerator FireRate()
    {
        canShoot = false;
        weaponAnimator.speed = fireRate;
        StartCoroutine(ShootDelayAnimation());
        yield return new WaitForSeconds(1/fireRate);
        weaponAnimator.SetBool("Shooting",false);
        weaponAnimator.speed = 1;
        canShoot = true;
    }

    private IEnumerator ShootDelayAnimation()
    {
        yield return new WaitForSeconds((1/fireRate)/4);
        weaponAnimator.SetBool("Shooting",true);
        flickLight.Flick();
    }
    
    private IEnumerator Reload()
    {
        reloadTime = reloadSound.length;
        weaponAudio.PlayOneShot(reloadSound,0.7f);
        weaponStatus = WeaponStatus.Reloading;
        evBullets?.Invoke(weaponStatus,currentBullets,maxBullets);
        canShoot = false;
        weaponAnimator.speed = reloadTime - reloadTime/10f;
        weaponAnimator.SetBool("Reloading",true);
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
        currentBullets = maxBullets;
        weaponStatus = WeaponStatus.Idle;
        evBullets?.Invoke(weaponStatus,currentBullets,maxBullets);
        weaponAnimator.speed = 1;
        weaponAnimator.SetBool("Reloading",false);
    }
    private void SpawnBulletHole()
    {
        var hole = bulletHoleObjectPool.GetObject();
        //hole.transform.parent = null;
        //hole.transform.localScale = Vector3.one;
        //hole.transform.parent = aimHit.transform;
        hole.transform.position = aimHit.point;
        hole.transform.rotation = Quaternion.FromToRotation(Vector3.forward, aimHit.normal);
    }
    private void ResetValues()
    {
        shooting = false;
        canShoot = true;
        currentBullets = maxBullets;
        shotsFired = 0;
        shotsHit = 0;
        evBullets?.Invoke(weaponStatus,currentBullets,maxBullets);
    }
}

public enum WeaponStatus
{
    Idle,
    Reloading,
    Shooting
}
}
