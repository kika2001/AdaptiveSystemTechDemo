using System;
using AdaptiveSystemDemo.Weapon;
using UnityEngine;

namespace AdaptiveSystemDemo.Health
{
    public class HealthSystem : MonoBehaviour, ITakeDamage
    {
        [SerializeField]protected int maxHealth;
        [SerializeField]protected int currentHealth;
        [SerializeField]protected ParticleSystem hitParticle;
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
            hitParticle.transform.forward = hitpoint.normal;
            hitParticle.transform.position = hitpoint.point;
        
            hitParticle.Play();
            if (Health==0)
            {
                evOnDie?.Invoke();
            }
        
        }
    }

}
