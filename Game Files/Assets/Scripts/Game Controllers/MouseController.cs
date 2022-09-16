using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : GameController
{
    
    //*******************************************************
    //Items to keep track of
    //*******************************************************
    public static Unit currentUnit;
    private static Unit selectedUnit;
    private static Unit previousUnit;

    public static int PlayerNumber = 0;

    private static HexagonTile currentHexagon;
    private static HexagonTile previousHexagon;
    private static List<HexagonTile> radiusHex = new List<HexagonTile>();

    private static bool startGame = false;
    private static bool objectPickedUp = false;

    private static GameObject pickedUpObject;
    private static Vector3 mouseLocation;

    //*******************************************************
    //Methods
    //*******************************************************


    //*******************************************************
    //Mouse Access Methods
    //*******************************************************
    private static GameObject RayPointsToObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.transform.gameObject;
        }
        return null;
    }

    private static void getRayPointsToLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            mouseLocation = hitInfo.transform.position;
        }
    }

 
    private static void highlightMouseOver(GameObject hitObject)
    {
        if (previousHexagon == null)
            previousHexagon = currentHexagon;
        
        currentHexagon = hitObject.GetComponent<HexagonTile>();
        if (previousHexagon != currentHexagon)
        {
            /*
            if (GameController.getAttackPhase())
            {
                
                foreach (HexagonTile hex in radiusHex)
                {
                    hex.removeMouseHighLight();
                }
                radiusHex = ActionController.findAttackable(currentHexagon, activeUnit.getAbility()[abilityChosen].getDamageRadius());
                foreach (HexagonTile hex in radiusHex)
                {
                    hex.mouseHighLight(Color.blue);
                }
                
                previousHexagon.removeHighlight();
                previousHexagon = currentHexagon; 
                return;
            }
            */
            previousHexagon.removeHighlight();
            previousHexagon = currentHexagon;
        }
        currentHexagon.mouseHighLight(Color.green);        
    }
    

    //*******************************************************
    //Game set-up methods
    //*******************************************************
    public static void setStartGame(bool SG)
    {
        startGame = SG;
    }

    public static GameObject getPickedUpObject()
    {
        return pickedUpObject;
    }


    public static void giveMouseAnObject(GameObject gameObject)
    {
        pickedUpObject = gameObject;
        objectPickedUp = true;
    }

    private static void PlaceUnit()
    {
        DestroyObject(pickedUpObject);
        objectPickedUp = false;
        return;
    }

    private static void MovePickedUpUnit(GameObject hitObject)
    {
        Unit unit = pickedUpObject.GetComponent<Unit>();
        HexagonTile tile = hitObject.GetComponent<HexagonTile>();
        unit.xCoordinate = tile.x;
        unit.yCoordinate = tile.y;
        //MapController.landObjectOnTile(tile.x, tile.y, pickedUpObject);
        //InstantiatePlayerUnit(unit, SetupController.getCurrentUnitSlot());
        pickedUpObject = null;
        objectPickedUp = false;
        return;
    }

    

    //*******************************************************
    //Run-time components
    //*******************************************************
    void Start()
    {
        /*
        players = new Player[2];
        for (int i = 0; i < 2; i++)
        {
            players[i] = new Player();
            players[i].playerNumber = i;
            players[i].setMaxArmy(MAX_UNITS);
        }

        */
        mouseLocation = new Vector3(0, 0, 0);
       
        players = playersChosen;
    }

    void Update()
    {
        getRayPointsToLocation();
        GameObject hitObject = RayPointsToObject();
        if (previousUnit != null)
            previousUnit.currentTile.removeHighlight();
        if (activeUnit != null)
        {
            previousUnit = activeUnit;
            activeUnit.currentTile.Highlight(Color.green);
        }

        if (movePhase)
        {
            if (hasMovedThisTurn) //can only move once a turn
            {
                movePhase = false;
                attackPhase = true;
            }
            activeUnit.HighlightMoveable();
            if (Input.GetMouseButtonDown(0)) //Left click
            {
                movePhase = true;
                activeUnit.isMoving = true;
                hasMovedThisTurn = true;
                //If successfully moved then end move phase
                if (activeUnit.Move(currentHexagon) )
                {
                    movePhase = false;
                    activeUnit.RemoveHighlightMovable();
                }
            }
            selectPhase = true;
        }
        else if (attackPhase)
        {
            if( previousAbility != abilityChosen)
            {
                activeUnit.RemoveHighlightAttackable();
                activeUnit.GetAttackable(abilityChosen);
            }

            if (abilityChosen != -1)
            {       
                activeUnit.HighlightAttackable();
            }
            else activeUnit.RemoveHighlightAttackable();

            if (Input.GetMouseButtonDown(0) && abilityChosen != -1) //Left click, ability is selected
            {
                //If successfully moved then end move phase
                if (activeUnit.Attack(currentHexagon, abilityChosen))
                {
                    abilityChosen = -1;
                    activeUnit.RemoveHighlightAttackable();
                    getNextUnit();
                    startSelectPhase();
                }
                
            }
            previousAbility = abilityChosen;

        }
        //MAY NEED A CONFIRMATION YES OR CANCEL ON ABILITY PRESS
        else if (selectPhase)
        {
            activeUnit.RemoveHighlightAttackable();
            activeUnit.RemoveHighlightMovable();
        }

        //*********************************************************
        //MOUSE INTERACTIONS
        //*********************************************************
        
        if (hitObject != null)
        {
            currentHexagon = hitObject.GetComponent<HexagonTile>();
            if (currentHexagon != null)
                highlightMouseOver(hitObject);
        }



        //*******************************************************
        //For unit placement
        //*******************************************************
        if (objectPickedUp)
        {
            pickedUpObject.transform.position = mouseLocation;
            pickedUpObject.transform.Translate(0, 0.5f, 0);

            if (Input.GetMouseButtonDown(0) && hitObject != null) MovePickedUpUnit(hitObject);
            else if (Input.GetMouseButtonDown(1)) PlaceUnit();      
        }
    }

}
