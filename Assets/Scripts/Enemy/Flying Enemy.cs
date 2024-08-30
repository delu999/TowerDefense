using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    public class FlyingEnemy : Enemy
    {
        protected override IEnumerator CalculatePathCoroutine()
        {
            // FlyingEnemy ignore obstacles and go directly to the target 
            _path = new List<Vector2> { _targetPosition };
            yield break;
        }
    }
}