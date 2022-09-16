using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Ability
{

    public Stun():base("Stun", 7, 0, "Does small damage (10-25% normal attack), stuns target for one round (they do not get their next turn). - 3 turn cooldown", 1)
    {

    }

    public override bool activate(HexagonTile destTile, Unit unitStats)
    {
        if (coolDown != 0)
        {
            return false;
        }
        List<HexagonTile> targetTile = ActionController.findMovable(destTile, 1);
        foreach (HexagonTile tile in targetTile)
        {
            if(tile.getHoldingUnit() != null)
            {
                tile.getHoldingUnit().takeDamage(unitStats.getAttack());
            }
        }
        return true;
    }

    public override void deactivate(Unit unitStats)
    {
        
    }
}
