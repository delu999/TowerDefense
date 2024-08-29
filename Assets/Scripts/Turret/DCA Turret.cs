using System.Collections;
using UnityEngine;

public class DCA : Turret
{
    protected override void Init()
    {
        name = "Defense Control Artillery";
        cost = 30;
        range = 4f;
        fireRate = 1f; // Average speed
        damage = 20;
    }

    protected override void Shoot()
    {
        if (_targetEnemy is null) return;

        StartCoroutine(MultipleMissile());
    }

    private IEnumerator MultipleMissile()
    {
        int i = 0;
        while (i < 4)
        {
            i++;
            if (_targetEnemy is null || _targetEnemy.gameObject is null)
            {
                _targetEnemy = null;
                yield break;
            }
            
            Bullet bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            bulletObj.Init( firingPoint, _targetEnemy.transform, GetRange(), damage/4);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public override float GetRange()
    {
        return 4f/2;
    }
}
