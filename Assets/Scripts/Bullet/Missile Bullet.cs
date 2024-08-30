using System.Collections.Generic;
using UnityEngine;

public class MissileBullet : Bullet
{
    protected float impactArea = 1f;
    [SerializeField] protected LayerMask enemyMask;
    
    public void Init(Transform origin, Transform target, float range, float damage) {
        base.Init(origin, target, range, damage);
        bulletSpeed /= 2f;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        List<Enemy> enemiesInImpactArea = FindTargets();

        foreach (var enemy in enemiesInImpactArea)
        {
            if (enemy is not null) {
                enemy.TakeDamage(GetDamage());
                Destroy(gameObject);
            }
        }
    }

    private List<Enemy> FindTargets()
    {
        List<Enemy> enemiesInImpactArea = new List<Enemy>(); 
        RaycastHit2D[] hits =
            Physics2D.CircleCastAll(transform.position, impactArea, transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                Enemy e = hit.transform.GetComponent<Enemy>();
                if (e is null) continue;
                
                enemiesInImpactArea.Add(e);
            }
        }

        return enemiesInImpactArea;
    }
}
