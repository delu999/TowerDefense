using System.Collections;
using UnityEngine;

namespace Turret
{
    public class Dca : Turret
    {
        protected override void Shoot()
        {
            if (TargetEnemy is null) return;

            StartCoroutine(MultipleMissile());
        }

        private IEnumerator MultipleMissile()
        {
            for (int i = 0; i < 4; i++)
            {
                if (TargetEnemy is null) yield break;
            
                Bullet.Bullet bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
                bulletObj.Init( firingPoint, TargetEnemy.transform, GetRange(), Damage/4);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
