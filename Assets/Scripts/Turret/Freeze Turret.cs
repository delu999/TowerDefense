using System.Collections;
using UnityEngine;

public class FreezeTurret : Turret
{
    private float _freezeTime;
    
    void Start()
    {
        name = "Freeze Turret";
        cost = 50;
        range = 3f;
        fireRate = 1.5f; // Slow speed
        damage = 0.1f;
        _freezeTime = 1f;
    }

    private void Update() {
        _fireCountdown -= Time.deltaTime;

        if (_fireCountdown > 0f) return;
        
        FreezeEnemies();
        _fireCountdown = 1f / fireRate;
    }

    private void FreezeEnemies() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, GetRange(), (Vector2) transform.position, 0f, enemyMask);

        if (hits.Length > 0) {
            for(int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];

                Enemy e = hit.transform.GetComponent<Enemy>();
                e.ChangeSpeedFactor(0.5f);
                _targetEnemy = e;
                Shoot();

                StartCoroutine(ResetEnemySpeed(e));
            }
        }
    }
    
    private IEnumerator ResetEnemySpeed(Enemy e) {
        yield return new WaitForSeconds(_freezeTime);

        e.ChangeSpeedFactor();
    }
}
