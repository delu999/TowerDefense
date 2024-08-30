using System;
using System.Collections;
using UnityEngine;

namespace Turret
{
    public class FreezeTurret : Turret
    {
        [SerializeField] private float freezeTime;

        private void Update() {
            if (FireCountdown > 0f)
            {
                FireCountdown -= Time.deltaTime;
                return;
            }
            
            FireCountdown = 1f/FireRate;
            FreezeEnemies();
        }

        private void FreezeEnemies()
        {
            RaycastHit2D[] hits = new RaycastHit2D[50];
            var hitsNumber = Physics2D.CircleCastNonAlloc(transform.position, GetRange(), transform.position, hits, 0f, enemyMask);

            if (hitsNumber <= 0) return;
            foreach (var hit in hits)
            {
                Enemy.Enemy e;
                try {
                    e = hit.transform.GetComponent<Enemy.Enemy>();
                } catch (Exception ex) {
                    continue;
                }
                if (e is null) continue;
                e.ChangeSpeedFactor(0.5f);
                TargetEnemy = e;
                Shoot();

                StartCoroutine(ResetEnemySpeed(e));
            }
        }
    
        private IEnumerator ResetEnemySpeed(Enemy.Enemy e) {
            yield return new WaitForSeconds(freezeTime);

            e.ChangeSpeedFactor();
        }
    }
}
