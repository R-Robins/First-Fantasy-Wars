using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marionette : Ability
{
    public Marionette() : base("Marionette", 16, 3, "Move another unit", 0) { }

    public override bool activate(HexagonTile target, Unit castingUnit)
    {
        return false;
    }

    public override void deactivate(Unit unitStats)
    {

    }
}
