using UnityEngine;
using UnityEngine.Tilemaps;

public class FastEnemy : Enemy
{
    protected override float GetSpeed()
    {
        return base.GetSpeed() * 2f; // Double the speed for fast enemies
    }

    public override void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        maxHealth = 35;
        base.Init(spawnPosition, targetPosition, ground);
    }
}