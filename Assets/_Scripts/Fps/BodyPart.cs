using System.Collections;
using System.Collections.Generic;
using AdaptiveSystemDemo.Health;
using AdaptiveSystemDemo.Weapon;
using UnityEngine;

namespace AdaptiveSystemDemo.Enemy
{
    public class BodyPart : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private float partDamageMultiplier;

        public void TakeDamage(int amount, RaycastHit point)
        {
            healthSystem.TakeDamage((int) Mathf.CeilToInt(amount * partDamageMultiplier),point);
        }
    }
}

