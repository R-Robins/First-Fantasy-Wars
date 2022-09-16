using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Ability {
    private List<Unit> targetUnits;

    public Charge():base("Charge", 9, 3, "Charge towards unit, range 3. Usable if adjacent tile to target is unoccupied, does basic attack damage - 3 turn cooldown", 1)
    {

    }

    public override bool activate(HexagonTile destTile, Unit unitStats)
    {
        unitStats.GetMoveable();
        foreach(HexagonTile tile in unitStats.GetMoveable())
        {
            if (destTile == tile)
            {
                if (coolDown != 0)
                {
                    return false;
                }
                unitStats.speed *= 8.0f;
                unitStats.animator.speed *= 8.0f;
                List<HexagonTile> targetTile = ActionController.findMovable(destTile, 1);
                foreach (HexagonTile effectTile in targetTile)
                {
                    if (effectTile.getHoldingUnit() != null)
                    {
                        targetUnits.Add(effectTile.getHoldingUnit());
                    }
                }
                unitStats.MoveToAttack(destTile);
                return true;
            }
        }
        return false;
    }

    public override void deactivate(Unit unitStats)
    {
        unitStats.speed /= 8.0f;
        unitStats.animator.speed /= 8.0f;
        for(int i = 0; i < targetUnits.Count; i++)
        {
            targetUnits[i].takeDamage(unitStats.getAttack());
        }
    }
}
