using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTargetAbility : Ability
{
    public string visualPath;
    public static Object visualPrefab;// = Resources.Load("Visual/Projectiles/Fireball");


    public OnTargetAbility(string visualPath, int range, string abilityName, int abilityID, string description) : base(abilityName, abilityID, range, description, 0)
    {
        this.visualPath = visualPath;
        this.range = range;
    }

    public override bool activate(HexagonTile targetTile, Unit castingUnit)
    {
        visualPrefab = Resources.Load(visualPath);
        GameObject projectile = Instantiate(visualPrefab) as GameObject;
        TargetedVisual targetVisual = projectile.GetComponent<TargetedVisual>();
        targetVisual.PlaceVisual(targetTile);
        return true;
    }

    public override void deactivate(Unit unitStats)
    {

    }
}
