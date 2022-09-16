using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViciousMockery : Ability
{

    public ViciousMockery() : base("ViciousMockery", 14, 4, "Insult your foe!", 0) { }

    public override bool activate(HexagonTile target, Unit castingUnit)
    {
        castingUnit.PopText("YOU WILL NEVER WIN!!!!");
        return true;
    }

    public override void deactivate(Unit unitStats)
    {

    }
}
