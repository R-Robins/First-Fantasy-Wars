using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
	public static HexagonTile[,] hexagonTiles;
	public static HexagonTile currentTile;

	public int passedWidth;
	public int passedHeight;

	public static int mapWidth;
	public static int mapHeight;

	//Maps the Tiles
	void Start()
	{
		mapWidth = passedWidth;
		mapHeight = passedHeight;
		createTileMap ();
	}

	public static void createTileMap()
	{
		hexagonTiles = new HexagonTile[mapWidth, mapHeight];
		for (int width = 0; width < mapWidth; width++) 
		{
			for (int height = 0; height < mapHeight; height++) 
			{
				try{
				currentTile = GameObject.Find ("Hexagon" + width + "|" + height).GetComponent<HexagonTile> ();
				if (currentTile != null) 
					hexagonTiles [width, height] = currentTile;
				}catch(System.Exception exception){
				}
			}
		}
	}

	//public static void landObjectOnTile(int tileX, int tileY, GameObject gameObject)
	//{
	//	HexagonTile tile = hexagonTiles [tileX, tileY];
	//	if (tile.transform.childCount != 0) 
	//	{
	//		Unit unit = tile.transform.GetChild (0).GetComponent<Unit> ();
	//		SetupController.setSelectedUnit (SetupController.getCurrentUnitSlot (), true);
	//		gameObject.transform.position = tile.transform.position;
	//		gameObject.transform.parent = tile.transform;
	//		gameObject.transform.Translate (0, 0.5f, 0, Space.World);
	//	}
	//}
}
