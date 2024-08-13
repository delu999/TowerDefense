using UnityEngine;
using UnityEngine.Tilemaps;

public class FlyingEnemy : Enemy
{
    public override void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        maxHealth = 50;
        reward = 5;
        base.Init(spawnPosition, targetPosition, ground);
    }

    // Additional logic to make this enemy only targetable by specific turrets
    // This can be managed in the Turret class by checking if the target is a FlyingEnemy
}