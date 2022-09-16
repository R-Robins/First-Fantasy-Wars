using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorTwo : MonoBehaviour {
	const int NUMBER_TILES = 6;
	public Transform[] hexagonTiles; 
	private HexagonTile tileProperties;

	public int gridWidth = 11;  //Number of items in grid (horizontal)
	public int gridHeight = 11; //Number of items in grid (vertical)

	float hexWidth = 1.732f;  //Tile Width
	float hexHeight = 2.0f; //Tile Height
	public float gap = 0.0f; //Gap between tiles

	public Transform tile1;
	public Transform tile2;
	public Transform tile3;
	public Transform tile4;
	public Transform tile5;
	public Transform tile6;


	Vector3 startPosition;  //Position to start grid generation

	void Start() //Creates everything 
	{
		hexagonTiles = new Transform[]{tile1,tile2,tile3,tile4,tile5,tile6};
		AddGap();
		CalcStartPosition();
		CreateGrid();
	}

	void CalcStartPosition() //Finds starting position to center grid
	{
		float offset = 0;
		if (gridHeight / 2 % 2 != 0) //On odd lines, offsets
			offset = hexWidth / 2;

		float x = -hexWidth * (gridWidth / 2) - offset;
		float z = hexHeight * .75f * (gridHeight / 2);

		startPosition = new Vector3 (x, 0, z);
	}


	Vector3 CalculateWorldPosition(Vector2 gridPosition) //Calculates placement
	{
		float offset = 0;
		if (gridPosition.y % 2 != 0)
			offset = hexWidth / 2;

		float x = startPosition.x + gridPosition.x * hexWidth + offset;
		float z = startPosition.z - gridPosition.y * hexHeight * .75f;

		return new Vector3 (x, 0, z);
	}

	void AddGap() //Adds gaps
	{
		hexWidth += hexWidth * gap;
		hexHeight += hexHeight * gap;
	}


	void CreateGrid() //Creates the grid, assigns identifiers
	{
		
		//Fill the rest
		for (int y = 0; y < gridHeight; y++) 
		{
			for (int x = 0; x < gridWidth; x++) 
			{
				spawnTile (0, x, y);
			}
		}
	}
		
	void spawnTile(int type, int x, int y)
	{
		if( GameObject.Find("Hexagon" + x + "|" + y) != null ) return;

		Transform hexagon = Instantiate (hexagonTiles[type]) as Transform;
		Vector2 gridPosition = new Vector2 (x, y);
		hexagon.position = CalculateWorldPosition (gridPosition);
		hexagon.parent = this.transform;
		hexagon.name = "Hexagon" + x + "|" + y;

		tileProperties = hexagon.GetComponent<HexagonTile>();
		tileProperties.x = x;
		tileProperties.y = y;
	}

}

