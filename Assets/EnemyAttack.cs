using System;
using System.Collections;
using System.Collections.Generic;
using AdaptiveSystemDemo.Character;
using AdaptiveSystemDemo.Health;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float distanceToAttack;
    [SerializeField] private int damage;
    [SerializeField] private int betweenAttacksTime;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private AudioClip idleClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> attackSounds;
    [SerializeField] private float pushAmount;
    private bool canAttack;

    // Start is called before the first frame update
    private void OnEnable()
    {
        ResetValues();
    }

    private void ResetValues()
    {
        canAttack = true;
        _audioSource.Play();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerHealthSystem.instance.gameObject)
        {
            if (canAttack)
            {
                GiveDamage();
            }
            
        }
    }

    private void GiveDamage()
    {
        canAttack = false;
        var dist = (PlayerHealthSystem.instance.transform.position - transform.position);
        Ray ray = new Ray(transform.position,dist.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,playerMask))
        {
            PlayerHealthSystem.instance.TakeDamage(damage,hit);
            _impulseSource.GenerateImpulse();
            PlaySoundAttack();
            FpsController.instance.PushPlayer(transform.position,pushAmount);
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(betweenAttacksTime);
        canAttack = true;
    }

    private void PlaySoundAttack()
    {
        _audioSource.Stop();
        var sound = attackSounds[Random.Range(0, attackSounds.Count)];
        _audioSource.PlayOneShot(sound);
        StartCoroutine(ResetSound(sound.length));
    }

    private IEnumerator ResetSound(float time)
    {
        yield return new WaitForSeconds(time);
        _audioSource.Play();
    }
    
}
