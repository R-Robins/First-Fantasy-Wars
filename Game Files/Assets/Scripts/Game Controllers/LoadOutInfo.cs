using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOutInfo {
    public short classSelected; //contains a number for a class, 0: melee, 1: mage, 2: bard
    public short[] skills = new short[3]; //will contain a number in each index, each number is spell number that a spell is assigned to.
}
