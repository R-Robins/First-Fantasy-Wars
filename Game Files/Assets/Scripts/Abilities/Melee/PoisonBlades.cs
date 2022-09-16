using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBlades : OnTargetAbility
{
    public PoisonBlades() : base("PoisonBlades", 1, "Poison Blades", 4, "Strike with venomous blades!") { }

    public override bool activate(HexagonTile target, Unit castingUnit)
    {
        visualPrefab = Resources.Load(visualPath);
        GameObject projectile = Instantiate(visualPrefab) as GameObject;
        TargetedVisual targetVisual = projectile.GetComponent<TargetedVisual>();
        targetVisual.PlaceVisual(target);
        if (target.holdingUnit != null) target.holdingUnit.takeDamage(45);
        return true;
    }
}