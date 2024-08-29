using UnityEngine;
using UnityEngine.Tilemaps;

public class ShieldedEnemy : Enemy
{
    public override void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        maxHealth = 40;
        reward = 5;
        base.Init(spawnPosition, targetPosition, ground);
    }

    public override void TakeDamage(float damage)
    {
        //float reducedDamage = Mathf.Max(1, damage / 2);
        base.TakeDamage(damage);
    }
}