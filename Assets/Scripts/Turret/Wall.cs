using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Turret
{
    protected override void Init()
    {
        name = "Wall";
        cost = 2;
    }

    private void Update()
    {
        return;
    }
}
