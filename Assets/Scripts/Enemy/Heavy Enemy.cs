using UnityEngine;
using UnityEngine.Tilemaps;

public class HeavyEnemy : Enemy
{
    protected override float GetSpeed()
    {
        return base.GetSpeed() * 0.5f; // Half the speed for heavy enemies
    }

    public override void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        maxHealth = 30;
        reward = 50;
        base.Init(spawnPosition, targetPosition, ground);
    }
}
