using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrutalBlow : OnTargetAbility
{

    public BrutalBlow() : base("BrutalBlow", 1, "BrutalBlow", 4, "Deal a brutal blow!") { }

    public override bool activate(HexagonTile target, Unit castingUnit)
    {
        visualPrefab = Resources.Load(visualPath);
        GameObject projectile = Instantiate(visualPrefab) as GameObject;
        TargetedVisual targetVisual = projectile.GetComponent<TargetedVisual>();
        targetVisual.PlaceVisual(target);
        if (target.holdingUnit != null) target.holdingUnit.takeDamage(60);
        return true;
    }

    public override void deactivate(Unit unitStats)
    {
        
    }
}
