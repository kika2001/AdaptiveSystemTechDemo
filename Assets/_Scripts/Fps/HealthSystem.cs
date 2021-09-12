using System;
using AdaptiveSystemDemo.Character;
using AdaptiveSystemDemo.Weapon;
using UnityEngine;
using UnityEngine.VFX;

namespace AdaptiveSystemDemo.Health
{
    public class HealthSystem : MonoBehaviour, ITakeDamage
    {
        
        [SerializeField]protected int maxHealth;
        [SerializeField]protected int currentHealth;
        //[SerializeField]protected ParticleSystem hitParticle;
        public delegate void dgOnDie();
        protected event dgOnDie evOnDie;

        public virtual void OnEnable()
        {
            currentHealth = maxHealth;
        }

        public int Health
        {
            get => currentHealth;
            set
            {
                if (value>0 && value <=maxHealth)
                {
                    currentHealth = value;
                }else if (value>maxHealth)
                {
                    currentHealth = maxHealth;
                }
                else
                {
                    currentHealth = 0;
                }
            
            }
        }

        public virtual void TakeDamage(int amount, RaycastHit hitpoint)
        {
            Health -= amount;
            BloodParticle.particle.transform.forward = (transform.position - hitpoint.point).normalized;
            BloodParticle.particle.transform.position = hitpoint.point;
        
            BloodParticle.particle.Play();
            BloodParticle.instance.PlayBloodSound(hitpoint.point);
            if (Health==0)
            {
                evOnDie?.Invoke();
            }
        
        }
    }

}
