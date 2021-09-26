using System;
using System.Collections;
using AdaptiveSystemDemo.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace AdaptiveSystemDemo.Health
{
    public class EnemyHealth : HealthSystem
    {
        [SerializeField] private float deadTime=1f;
        public bool isDead = false;
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hit;
        public override void OnEnable()
        {
            base.OnEnable();
            movement.enabled = true;
            movement.agent.enabled = true;
            animator.SetBool("dead",false);
            animator.speed = movement.agent.speed/3.5f;
            audioSource.UnPause();
            isDead = false;
        }
    
        private void Awake()
        {
            evOnDie += OnevOnDie;
        }
    
        public override void TakeDamage(int amount, RaycastHit hitpoint)
        {
            base.TakeDamage(amount, hitpoint);
            if(isDead) return;
            audioSource.PlayOneShot(hit,0.5f);
        }
    
        private void OnevOnDie()
        {
            StartCoroutine(Die());
        }
    
        private IEnumerator Die()
        {
            if (isDead) yield break;
            isDead = true;
            movement.enabled = false;
            movement.agent.enabled = false;
            animator.SetBool("dead",true);
            audioSource.Pause();
            yield return new WaitForSeconds(deadTime);
            DungeonManager.enemiesGO.Remove(gameObject);
            gameObject.ReturnToPool();
            
    
    
        }
    }

}
