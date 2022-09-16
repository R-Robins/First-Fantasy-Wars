using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeights : MonoBehaviour 
{
	public int mapWidth;
	public int mapHeight;

	// Use this for initialization
	void Start () 
	{
		for (int x = 0; x < mapWidth; x++) 
		{
			for (int y = 0; y < mapHeight ; y++) 
			{
				//HexagonTile currentTile = GameObject.Find("Hexagon" + x + "|" + y).GetComponent<HexagonTile>();
				//currentTile.height = (int)currentTile.transform.localScale.z;
			}
		}
	}

}
