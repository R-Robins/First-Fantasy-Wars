using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Ability  : MonoBehaviour
{
    protected int abilityID;
    protected string abilityName;
    protected string description;
    protected int effectDuration = 0;
    protected int coolDown = 0;
    protected int range;
    protected int damageRadius;
    

    public Ability( string abilityName, int abilityID, int range, string description, int dmgRad )
    {
        this.abilityName = abilityName;
        this.abilityID = abilityID;
        this.range = range;
        this.description = description;
        damageRadius = dmgRad;
    }
    
    //An int to determine which skillslot is the skill embedded to during unit selection. 
    //The purpose is to disabled skill from choosing it again if it's already chosen.
    public int skillSlotNumber = -1;

    public int getDamageRadius()
    {
        return damageRadius;
    }

    public int getCoolDown()
    {
        return coolDown;
    }

    public int getSkillSlot()
    {
        return skillSlotNumber;
    }

    public void setSkillSlot(int c)
    {
        skillSlotNumber = c;
    }

    public void decrementDuration(Unit unit)
    {
        if((effectDuration--) <= 0)
        {
            effectDuration = 0;
            deactivate(unit);
        }
    }

    public void decrementCoolDown()
    {
        if((coolDown--) <= 0)
        {
            coolDown = 0;
        }
    }

    public int getRange()
    {
        return range;
    }

    public string getDescription()
    {
        return this.description;
    }

    public string getName()
    {
        return this.abilityName;
    }

    public abstract void deactivate(Unit unitStats);
    public abstract bool activate( HexagonTile targetTile, Unit castingUnit);
    
    
}
