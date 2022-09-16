using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour 
{
	public const int MAX_UNITS = 5;
	public string playerName;
	public Unit[] playerUnits = new Unit[MAX_UNITS];
	public bool isActive = false;
	public int playerNumber;

	public Unit getUnit(int unitNumber)
	{
		return playerUnits [unitNumber];
	}

	public bool isAllUnitsPlaced()
	{
		for (int j = 0; j < MAX_UNITS; j++) 
		{
			if (playerUnits [j] == null)
				return false;
		}
		return true;	
	}

	public void DestroyUnit(int index)
	{
		DestroyObject (playerUnits [index].gameObject);
	}

	public void setUnitsOnGameBoard()
	{
        int[] x = { 0, 1, 2, 3, 4 };
        int[] y = { 0, 0, 0, 0, 0 };
        for (int j = 0; j < MAX_UNITS; j++)
        {
            if (playerUnits[j].getCharacter() != null)
            {
                playerUnits[j].xCoordinate = x[j];
                playerUnits[j].yCoordinate = y[j];
                playerUnits[j].placeUnit();
            }
        }
    }

    public void setUnitsOnGameBoard(int[,] coordinates )
    {
        for (int unitIndex = 0; unitIndex < 5; unitIndex++)
        {
            if(playerUnits[unitIndex].getCharacter() != null)
            {
                playerUnits[unitIndex].xCoordinate = coordinates[unitIndex, 0];
                playerUnits[unitIndex].yCoordinate = coordinates[unitIndex, 1];
                playerUnits[unitIndex].placeUnit();
            }
        }
    }

	public void setMaxArmy(int armySize)
	{
		this.playerUnits = new Unit[armySize];
	}

	public void setUnit( int unitNumber, Unit unit)
	{
		playerUnits [unitNumber] = unit;
	}

    public void setArmy(Unit[] army)
    {
        playerUnits = army;
    }
}
