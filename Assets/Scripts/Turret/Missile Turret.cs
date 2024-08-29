using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : Turret
{
    protected override void Init()
    {
        name = "Missile Turret";
        cost = 20;
        range = 6f;
        fireRate = 2f;
        damage = 8;
    }
    
    public override float GetRange()
    {
        return 6f/2;
    }
}
