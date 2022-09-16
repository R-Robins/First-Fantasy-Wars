using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisChord : OnTargetAbility
{

    public DisChord() : base("DisChord", 3, "DisChord", 4, "Attack with sound!") { }

    public override bool activate(HexagonTile target, Unit castingUnit)
    {
        visualPrefab = Resources.Load(visualPath);
        GameObject projectile = Instantiate(visualPrefab) as GameObject;
        TargetedVisual targetVisual = projectile.GetComponent<TargetedVisual>();
        targetVisual.PlaceVisual(target);
        if (target.holdingUnit != null) target.holdingUnit.takeDamage(30);
        return true;
    }

    public override void deactivate(Unit unitStats)
    {

    }
}
