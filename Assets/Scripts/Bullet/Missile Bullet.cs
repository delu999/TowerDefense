using System.Collections.Generic;
using UnityEngine;

namespace Bullet
{
    public class MissileBullet : Bullet
    {
        protected float impactArea = 0.5f;
        [SerializeField] protected LayerMask enemyMask;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            List<Enemy.Enemy> enemiesInImpactArea = FindTargets();

            foreach (var enemy in enemiesInImpactArea)
            {
                if (enemy is not null) {
                    enemy.TakeDamage(GetDamage());
                    Destroy(gameObject);
                }
            }
        }

        private List<Enemy.Enemy> FindTargets()
        {
            List<Enemy.Enemy> enemiesInImpactArea = new List<Enemy.Enemy>(); 
            RaycastHit2D[] hits =
                Physics2D.CircleCastAll(transform.position, impactArea, transform.position, 0f, enemyMask);

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];

                    Enemy.Enemy e = hit.transform.GetComponent<Enemy.Enemy>();
                    if (e is null) continue;
                
                    enemiesInImpactArea.Add(e);
                }
            }

            return enemiesInImpactArea;
        }
    }
}
