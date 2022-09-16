using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGUI : MonoBehaviour
{

    float classButtonSizeX = 30;
    float classButtonSizeY = 30;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnGUI()
    {
        if(GameController.getGameStart() == false)
        {
            if (GUI.Button(new Rect(10, 10, 60, 30), "START"))
            {
                GameController.StartGame();
                GameController.setSelectPhase(true);
                return;
            }
            
        }


        if (GameController.getGameStart())
        {
            if (GameController.getSelectPhase())
            {
                if (GameController.hasMovedThisTurn == false)
                {
                    if (GUI.Button(new Rect(10, 10, classButtonSizeX, classButtonSizeY), "M"))
                        GameController.startMovePhase();
                }

                if (GUI.Button(new Rect(10, 45, classButtonSizeX, classButtonSizeY), "A"))
                    GameController.startAttackPhase();

                if (GUI.Button(new Rect(10, 80, classButtonSizeX, classButtonSizeY), "X"))
                    GameController.startSelectPhase();

                if (GUI.Button(new Rect(10, 115, classButtonSizeX, classButtonSizeY), "S")) 
                    GameController.getNextUnit();                   
                

            }
            else if (GameController.getMovePhase())
            {
                if (GUI.Button(new Rect(10, 80, classButtonSizeX, classButtonSizeY), "X"))
                    GameController.startSelectPhase();
            }
            else if (GameController.getAttackPhase())
            {
                if (GUI.Button(new Rect(10, 10, classButtonSizeX, classButtonSizeY), "1"))
                {
                    GameController.setChosenAbility(0);
                }//need to pass 1

                if (GUI.Button(new Rect(10, 45, classButtonSizeX, classButtonSizeY), "2"))
                {
                    GameController.setChosenAbility(1);
                }//need to pass 2 

                if (GUI.Button(new Rect(10, 80, classButtonSizeX, classButtonSizeY), "3"))
                {
                    GameController.setChosenAbility(2);
                }
                if (GUI.Button(new Rect(10, 115, classButtonSizeX, classButtonSizeY), "X"))
                {
                    GameController.abilityChosen = -1;
                    GameController.deselectAbility();
                    GameController.startSelectPhase();    
                }
            }
        }
    }
}
