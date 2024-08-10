using UnityEngine;
using UnityEngine.Tilemaps;

public class BossEnemy : Enemy
{
    public override void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        maxHealth = 100;
        reward = 200;
        base.Init(spawnPosition, targetPosition, ground);
    }

    protected override float GetSpeed()
    {
        return base.GetSpeed() * 0.75f; // Slightly slower than normal enemies
    }
}
