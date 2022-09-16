using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard : Unit
{
    public Bard() : base("Bard", 100, 100, 20, 0, 3, 2)
    {
    }
    public void Start()
    {
        characterName = "Bard";
        maxHealth = 100;
        health = 100;
        attack = 20;
        defense = 5;
        move = 3;
        charNumber = 2;
    }
    public void Reset()
    {
        abilities[0] = new RestorativeSeronade();
        abilities[1] = new LamentOfFate();
        abilities[2] = new Marionette();

        availableAbilities[0] = new RestorativeSeronade();
        availableAbilities[1] = new LamentOfFate();
        availableAbilities[2] = new Marionette();

    }
}
