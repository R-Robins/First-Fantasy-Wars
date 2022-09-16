using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableSpells : MonoBehaviour {

    public static Ability[] meleeAbilities = new Ability[5];
	// Use this for initialization
	void Start () {
        meleeAbilities[0] = new Charge();
        meleeAbilities[0].skillSlotNumber = 0;
        meleeAbilities[1] = new Cover();
        meleeAbilities[1].skillSlotNumber = 1;
        meleeAbilities[2] = new PoisonBlades();
        meleeAbilities[2].skillSlotNumber = 2;
        meleeAbilities[3] = new BrutalBlow();
        meleeAbilities[3].skillSlotNumber = 3;
        meleeAbilities[4] = new Stun();
        meleeAbilities[4].skillSlotNumber = 4;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
