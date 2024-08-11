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
        fireRate = 2f; // Very slow speed
        damage = 8;
    }
}
