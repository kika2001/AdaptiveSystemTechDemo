using System;
using System.Collections;
using System.Collections.Generic;
using AdaptiveS.System;
using UnityEngine;

namespace AdaptiveSystemDemo.Health
{
    public class PlayerHealthSystem : HealthSystem
    {
        public static PlayerHealthSystem instance;

        private void Awake()
        {
            if (instance!=null)
            {
                Destroy(this.gameObject);
            }

            instance = this;
        }

        private AdaptiveSystem adaptiveSystem;
        private void Start()
        {
            adaptiveSystem=AdaptiveSystemManager.NewAdaptiveSystem("player");
            AdaptiveSystemManager.AddDataToAnalyse("health",currentHealth,maxHealth,adaptiveSystem);
        }

        private void Update()
        {
            AdaptiveSystemManager.UpdateInfo("health",currentHealth,adaptiveSystem);
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }
    }
 
}
