using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    
    
    public class BossEnemy : Enemy
    {
        [SerializeField] private WaveConfig.Wave enemiseInsideTank;
        protected override float GetSpeed()
        {
            return base.GetSpeed() * 0.75f; // Slightly slower than normal enemies
        }

        public override void TakeDamage(float damage)
        {
            if (enemiseInsideTank is not null && _currentHealth <= damage && EnemySpawner.Instance) {
                StartCoroutine(BossExplosion(damage));
                return;
            }
            base.TakeDamage(damage);
        }

        protected IEnumerator BossExplosion(float damage)
        {
            EnemySpawner.Instance.SpawnWaveFromSinglePoint(enemiseInsideTank, transform.position);
            yield return new WaitForSeconds(1f);
            base.TakeDamage(damage);
        }
    }
}
