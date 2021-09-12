using System;
using System.Collections;
using System.Collections.Generic;
using AdaptiveS.System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AdaptiveSystemDemo.Health
{
    public class PlayerHealthSystem : HealthSystem
    {
        public static PlayerHealthSystem instance;
        private bool invencible = false;
        [SerializeField] private float invencibleTime;
        [SerializeField] private AudioSource playerAudio;
        [SerializeField] private List<AudioClip> hurtSounds;

        private void Awake()
        {
            if (instance!=null)
            {
                Destroy(this.gameObject);
            }

            instance = this;
        }
        private void Start()
        {
            adaptiveSystem=AdaptiveSystemManager.NewAdaptiveSystem("player");
            AdaptiveSystemManager.AddDataToAnalyse("health",currentHealth,maxHealth,adaptiveSystem);
        }

        private void Update()
        {
            AdaptiveSystemManager.UpdateInfo("health",currentHealth,adaptiveSystem);
        }


        private AdaptiveSystem adaptiveSystem;
        public override void TakeDamage(int amount, RaycastHit hitpoint)
        {
            if (invencible) return;
            base.TakeDamage(amount, hitpoint);
            PlayRandomHurtSound();
            StartCoroutine(InvencibleTime());

        }
        

        private void PlayRandomHurtSound()
        {
            if (playerAudio.isPlaying) return;
            var sound = hurtSounds[Random.Range(0, hurtSounds.Count)];
            playerAudio.PlayOneShot(sound);

        }

        
        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        private IEnumerator InvencibleTime()
        {
            invencible = true;
            yield return new WaitForSeconds(invencibleTime);
            invencible = false;
        }
    }
 
}
