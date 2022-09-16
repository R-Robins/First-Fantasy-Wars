using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorativeSeronade : OnTargetAbility
{
    public RestorativeSeronade() : base("RestorativeSeronade", 3, "RestorativeSeronade", 4,  "Heal another unit") {}


    
    public override bool activate(HexagonTile target, Unit castingUnit)
    {
        visualPrefab = Resources.Load(visualPath);
        GameObject projectile = Instantiate(visualPrefab) as GameObject;
        TargetedVisual targetVisual = projectile.GetComponent<TargetedVisual>();
        targetVisual.PlaceVisual(target);
        if (target.holdingUnit != null) target.holdingUnit.heal(40);
        return true;
    }
    
    public override void deactivate(Unit unitStats)
    {

    }
    
}
