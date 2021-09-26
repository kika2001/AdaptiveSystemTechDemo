using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour
{
    public static ParticleSystem particle;
    public static BloodParticle instance;
    [SerializeField]private ObjectPool pool;
    private void Awake()
    {
        if (particle!=null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        particle = GetComponent<ParticleSystem>();
    }

    public void PlayBloodSound(Vector3 pos)
    {
        var obj = pool.GetObject();
        obj.transform.position = pos;
        obj.GetComponent<AudioSource>().Play();
    }
    void OnParticleCollision(GameObject other)
    {
        Debug.Log("PlayingSound");
        PlayBloodSound(other.transform.position);
    }
    
    
}
