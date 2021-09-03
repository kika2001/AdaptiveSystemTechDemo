
using UnityEngine;

namespace AdaptiveSystemDemo.Weapon
{
    public interface ITakeDamage
    {
        public void TakeDamage(int amount, RaycastHit point);
    }
}

