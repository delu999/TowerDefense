using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlyingEnemy : Enemy
{
    public override void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        maxHealth = 50;
        reward = 3;
        base.Init(spawnPosition, targetPosition, ground);
    }
    
    protected override IEnumerator CalculatePathCoroutine()
    {
        // FlyingEnemy ignore obstacles and go directly to the target 
        _path = new List<Vector2> { _targetPosition };
        yield break;
    }
}