using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : HealthSystem
{
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
