using System;
using System.Collections;
using System.Collections.Generic;
using AdaptiveS.System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace AdaptiveSystemDemo.Health
{
    public class PlayerHealthSystem : HealthSystem
    {
        public static PlayerHealthSystem instance;
        private bool invencible = false;
        [SerializeField] private float invencibleTime;
        [SerializeField] private AudioSource playerAudio;
        [SerializeField] private List<AudioClip> hurtSounds;
        [SerializeField] private List<Sprite> hurtsSprites;
        [SerializeField] private Image hurtImage;
        [SerializeField] private float timeToHurtDisappear=5f;
        private float currentHurtTime = 0f;
        private AdaptiveSystem adaptiveSystem;
        public UnityEvent onDie;
        private void Awake()
        {
            if (instance!=null)
            {
                Destroy(this.gameObject);
            }

            instance = this;
            evOnDie += OnDie;
        }

        private void OnDie()
        {
            onDie.Invoke();
        }

        private void Start()
        {
            adaptiveSystem=AdaptiveSystemManager.NewAdaptiveSystem("player");
            AdaptiveSystemManager.AddDataToAnalyse("health",currentHealth,maxHealth,adaptiveSystem);
        }

        
        public override void TakeDamage(int amount, RaycastHit hitpoint)
        {
            if (invencible) return;
            base.TakeDamage(amount, hitpoint);
            AdaptiveSystemManager.UpdateInfo("health",currentHealth,adaptiveSystem);
            PlayRandomHurtSound();
            ShowHurtUI();
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

        private void Update()
        {
            UpdateHurtUI();
        }

        private void UpdateHurtUI()
        {
            if (hurtImage.IsActive())
            {
                currentHurtTime += Time.deltaTime;
                hurtImage.color = new Color(255,255,255,1 - (currentHurtTime / timeToHurtDisappear));
                if (currentHurtTime>=timeToHurtDisappear)
                {
                    hurtImage.gameObject.SetActive(false);
                }
            }
        }
        private void ShowHurtUI()
        {
            currentHurtTime = 0;
            var wantedImageIndex = Mathf.RoundToInt(currentHealth / (maxHealth / hurtsSprites.Count));
            hurtImage.sprite = hurtsSprites[wantedImageIndex];
            hurtImage.gameObject.SetActive(true);
        }
    }
 
}
