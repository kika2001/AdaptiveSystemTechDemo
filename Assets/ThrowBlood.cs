using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBlood : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private ParticleSystem bloodParticleSystem;
    private BloodParticle bloodParticleSound;
    public List<ParticleCollisionEvent> collisionEvents;
    private bool firingBlood = false;
    private InputMaster controls;
    RaycastHit hit;
    Ray ray;
    // Start is called before the first frame update
    private void Awake()
    {
        camera = GetComponent<Camera>();
        bloodParticleSound = bloodParticleSystem.GetComponent<BloodParticle>();
        controls = new InputMaster();
        controls.Player.Enable();
        controls.Player.Fire.Enable();
        controls.Player.MousePosition.Enable();
    }
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        bloodParticleSystem.Play();
    }

    
    void Update()
    {
        ray = camera.ScreenPointToRay(controls.Player.MousePosition.ReadValue<Vector2>());
        Physics.Raycast(ray, out hit);
            
        bloodParticleSystem.transform.LookAt(hit.point);
        
        /*
        firingBlood = ((controls.Player.Fire.ReadValue<float>()>0) ? true : false);
        if (firingBlood)
        {
            ray = camera.ScreenPointToRay(controls.Player.MousePosition.ReadValue<Vector2>());
            Physics.Raycast(ray, out hit);
            
            bloodParticleSystem.transform.LookAt(hit.point);
            bloodParticleSystem.Play();
        }
        else
        {
            bloodParticleSystem.Stop();
        }
        */
    }
    

   
    
}
