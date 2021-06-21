using UnityEngine;

    public class Destructible : HealthSystem
    {
        public override void TakeDamage(int amount, RaycastHit hitpoint)
        {
            base.TakeDamage(amount, hitpoint);
            if (Health==0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.ReturnToPool();
                }
                Destroy(this.gameObject);
            }
        }

    }
