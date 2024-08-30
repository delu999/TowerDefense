using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    public class ShieldedEnemy : Enemy
    {
        [SerializeField] private float shieldLife;
        
        public override void TakeDamage(float damage)
        {
            if (shieldLife >= damage)
            {
                shieldLife -= Mathf.Max(1, damage * 0.5f);
                return;
            }
            float reducedDamage = Mathf.Max(1, damage * 0.9f);
            base.TakeDamage(reducedDamage);
        }
    }
}