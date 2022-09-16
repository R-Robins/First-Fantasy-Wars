using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Melee : Unit
{
    public Melee():base("Melee", 120, 120, 20, 5, 3, 0)
    {
    }

    public void Start()
    {
        characterName = "Melee";
        maxHealth = 120;
        health = 120;
        attack = 20;
        defense = 5;
        move = 3;
        charNumber = 0;
    }
}
