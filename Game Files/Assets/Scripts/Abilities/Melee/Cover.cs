using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : Ability
{
    private List<Unit> protectedUnits;
    public Cover():base("Cover", 8, 0, "Take damage instead of an ally until warriors next turn (any number of hits) - no cooldown", 1)
    {

    }

    public override bool activate(HexagonTile destTile, Unit unitStats)
    {
        List<HexagonTile> targetTile = ActionController.findMovable(destTile, 1);
        foreach (HexagonTile tile in targetTile)
        {
            if(tile.getHoldingUnit() != null)
            {
                if (unitStats.getProtector() != null && unitStats.getProtector().Equals(tile.getHoldingUnit()))
                {
                    unitStats.setProtector(null);
                }
                protectedUnits.Add(tile.getHoldingUnit());
                tile.getHoldingUnit().setProtector(unitStats);
            }
        }
        effectDuration = 1;
        return true;
    }

    public override void deactivate(Unit unitStats)
    {
        for(int i = 0; i < protectedUnits.Count; i++)
        {
            protectedUnits[i].setProtector(null);
        }
    }
}