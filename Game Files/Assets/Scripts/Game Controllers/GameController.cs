using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    //**********************
    //FIX THESE LATER
    //**********************
    public Player[] playersChosen = new Player[MAX_PLAYER];
    public static int winner = -1;

    //*******************************************************
    //Items to keep track of
    //*******************************************************
    public const int MAX_UNITS = 5;
	public const int MAX_PLAYER = 2;

    protected static int activePlayer = 0;
    public static Unit activeUnit;
	public static HexagonTile activeTile;
    public static int abilityChosen = -1;
    public static int previousAbility = -1;

    //protected static Player[] players = new Player[MAX_PLAYER];
    public static Player[] players = new Player[MAX_PLAYER];
    public static int[] unitIndex = new int[MAX_UNITS];

	protected static bool movePhase = false; //Select movement
    protected static bool attackPhase = false; //Select attack range
    protected static bool gameStart = false; //Game has started
    protected static bool selectPhase = false; //Select what action
    protected static bool abilitySelectPhase = false; //Select the ability to use
    public static bool gameOver = false; //Game has ended
    public static bool hasMovedThisTurn = false; //Has the current unit moved?

    //*******************************************************
    //Methods
    //*******************************************************
    public static void ImportPlayersToGame(Player player1, Player player2)
    {
        players[0] = player1;
        players[1] = player2;
    }

    public static void setChosenAbility(int i)
    {
        abilityChosen = i;
        activeUnit.RemoveHighlightAttackable();
        activeUnit.GetAttackable(abilityChosen);
        activeUnit.HighlightAttackable();
        activeUnit.RemoveHighlightMovable();
    }

    public static void deselectAbility()
    {
        activeUnit.RemoveHighlightAttackable();
    }

    //*******************************************************
    //Turn Flow
    //*******************************************************
    public static Unit getNextUnit()
    {
        int unitsParsed = 0;
        activeUnit.RemoveHighlightMovable();
        activeUnit.RemoveHighlightAttackable();
        hasMovedThisTurn = false;
        activePlayer = (activePlayer + 1) % MAX_PLAYER;
        unitIndex[activePlayer] = (unitIndex[activePlayer] + 1) % MAX_UNITS;
        activeUnit = players[activePlayer].getUnit(unitIndex[activePlayer]);

        while(activeUnit.isAlive != true)
        {
            unitsParsed++;
            if (unitsParsed > 5)
            {

                winner = ((activePlayer + 1) % MAX_PLAYER);
                gameOver = true;
            }
        }
        return activeUnit;
    }

    //*******************************************************
    //PHASE CHANGES
    //*******************************************************
    public static void StartGame()
    {
        gameStart = true;
        
        players[0] = new Player();
        players[0].setArmy(GameObject.Find("GameControllers").GetComponent<StaticInfo>().LoadUnits());
        players[0].setUnitsOnGameBoard();
        
        activeUnit = players[0].getUnit(0);
    }

    public static void setMovePhase()
    {
        activeUnit.GetMoveable();
        movePhase = true;
    }

    public static void usingAbilityPhase(int abilityNumber)
    {
        activeUnit.GetAttackable(abilityNumber);
    }

    //*******************************************************
    //Accessors
    //*******************************************************
    public static Unit getActiveUnit()
    {
        return activeUnit;
    }

    public static int getMaxUnits()
    {
        return MAX_UNITS;
    }

    public static bool getMovePhase()
    {
        return movePhase;
    }

    public static bool getAttackPhase()
    {
        return attackPhase;
    }

    public static bool getGameStart()
    {
        return gameStart;
    }

    public static bool getSelectPhase()
    {
        return selectPhase;
    }

    public static bool getAbilitySelectPhase()
    {
        return abilitySelectPhase;
    }
    //*******************************************************
    //Phase Changing Methods
    //*******************************************************
    public static void setMovePhase(bool boolean)
    {
        movePhase = boolean;
    }

    public static void setAttackPhase(bool boolean)
    {
        attackPhase = boolean;
    }

    public static void setGameStart(bool boolean)
    {
        gameStart = boolean;
    }

    public static void setSelectPhase(bool boolean)
    {
        selectPhase = boolean;
    }
    
    public static void setAbilitySelectPhase(bool boolean)
    {
        abilitySelectPhase = boolean;
    }

    public static void startMovePhase()
    {
        movePhase = true;
        selectPhase = false;
        attackPhase = false;
        abilitySelectPhase = false;
        activeUnit.RemoveHighlightAttackable();

        activeUnit.GetMoveable();
    }

    public static void startAttackPhase()
    {
        attackPhase = true;
        movePhase = false;
        selectPhase = false;
        abilitySelectPhase = false;
    }

    public static void startSelectPhase()
    {
        selectPhase = true;
        attackPhase = false;
        movePhase = false;
        abilitySelectPhase = false;
    }

    public static void startAbilitySelectPhase()
    {
        selectPhase = false;
        attackPhase = false;
        movePhase = false;
        abilitySelectPhase = true;
    }
    //*******************************************************
    //Unit Placement
    //*******************************************************

    public static void InstantiatePlayerUnit(Unit unit, int index)
    {
        
        if (players[activePlayer].getUnit(index) != null)
        {
            players[activePlayer].DestroyUnit(index);
        }
        players[activePlayer].setUnit(index, unit);
    }

    public static bool isAllCharInPlaced()
    {
        for (int j = 0; j < MAX_PLAYER; j++)
        {
            if (!players[j].isAllUnitsPlaced())
            {
                return false;
            }
        }
        return true;
    }

    public static void PlaceUnits()
    {
        for (int i = 0; i < MAX_PLAYER; i++)
            players[i].setUnitsOnGameBoard();
    }

    public static void setActivePlayer(int activeP)
    {
        activePlayer = activeP;
    }


    //*******************************************************
    //Run time components
    //*******************************************************

    // Use this for initialization
    void Start () 
	{
        /*
        //players = new Player[MAX_PLAYER];
		for (int i = 0; i < MAX_PLAYER; i++) 
		{
            players[i] = new Player ();
            players[i].playerNumber = i;
			players[i].setMaxArmy (MAX_UNITS);
		}
        */

	}
}
