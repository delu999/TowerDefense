using UnityEngine;
using UnityEngine.Tilemaps;

public class ShieldedEnemy : Enemy
{
    public override void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        maxHealth = 20;
        reward = 40;
        base.Init(spawnPosition, targetPosition, ground);
    }

    public override void TakeDamage(int damage)
    {
        int reducedDamage = Mathf.Max(1, damage / 2); // Example: reduces damage by half, but at least 1
        base.TakeDamage(reducedDamage);
    }
}