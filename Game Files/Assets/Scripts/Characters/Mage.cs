using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Unit
{
    public Mage() : base("Mage", 100, 100, 5, 0, 2, 1)
    {
    }

    public void Start()
    {
        characterName = "Mage";
        maxHealth = 80;
        health = 80;
        attack = 5;
        defense = 0;
        move = 2;
        charNumber = 1;
    }
    public void Reset()
    {
        abilities[0] = new ArcaneMissiles();
        abilities[1] = new Fireball();
        abilities[2] = new GlacialSpike();

        availableAbilities[0] = new ArcaneMissiles();
        availableAbilities[1] = new Fireball();
        availableAbilities[2] = new GlacialSpike();
    }

}
