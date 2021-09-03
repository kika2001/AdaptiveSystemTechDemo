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
        private bool isDead = false;
        [SerializeField] private EnemyStatsTest stats;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hit;
        public override void OnEnable()
        {
            base.OnEnable();
            stats.enabled = true;
            agent.enabled = true;
            animator.SetBool("dead",false);
            animator.speed = agent.speed/3.5f;
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
            stats.enabled = false;
            agent.enabled = false;
            animator.SetBool("dead",true);
            audioSource.Pause();
            //transform.up = transform.forward;
            //transform.position = transform.position - transform.up;
            //transform.localRotation = Quaternion.Euler(transform.localRotation.x-90,transform.localRotation.y,transform.localRotation.z);
            yield return new WaitForSeconds(deadTime);
            EnemySpawner.enemiesGO.Remove(gameObject);
            gameObject.ReturnToPool();
            
    
    
        }
    }

}
