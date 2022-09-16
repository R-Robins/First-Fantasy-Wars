using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Ability {

    // Use this for initialization
    public Teleport() : base("Teleport", 5, 4, "Instantly land on specified location", 0)
    {

    }

    public override bool activate(HexagonTile targetTile, Unit castingUnit)
    {
        return true;
    }

    public override void deactivate(Unit unitStats)
    {
    }
}
