using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapGenerator : MonoBehaviour {

    public GameObject hexagonTile;
    private List<GameObject[]> tiles;
    private int mapHeight = 12;
    float hexWidth = 1.732f;  
    float hexHeight = 2.0f;

    // Use this for initialization
    void Start () {
        tiles = new List<GameObject[]>();
    }
	
	// Update is called once per frame
	void Update () 
	{
    }

    public void GenerateMap()
    {
        float gapX = hexWidth/4;
        float gapY = hexHeight * .75f;
        int sectionWidth;
        for (int i = -mapHeight; i <= mapHeight; i++)
        {
            gapX *= -1;
            sectionWidth = (int)Random.Range(3, 13);
            GameObject[] tileObjects = new GameObject[mapHeight * 2 + 1];
            for(int j = -sectionWidth; j <= sectionWidth; j++)
            {
                tileObjects[j + mapHeight] = Instantiate(hexagonTile) as GameObject;
                tileObjects[j + mapHeight].transform.position = new Vector3(hexWidth * j + gapX, 0, gapY * i);
                HexagonTile hex = tileObjects[j + mapHeight].GetComponent<HexagonTile>();
                hex.x = j + mapHeight;
                hex.y = i + mapHeight;
                string name = "Hexagon" + (j + mapHeight).ToString() + "|" + (i + mapHeight).ToString();
                hex.name = name;
                tileObjects[j + mapHeight].name = name;
            }
            tiles.Add(tileObjects);
        }
		MapController.createTileMap ();
        //attachSurroundingTiles();
        //GetComponent<MapController>().setMap(tiles);
    }

	/*
    public void attachSurroundingTiles()
    {
        for(int y = 0; y < mapHeight*2 + 1; y++)
        {
            for(int x = 0; x < mapHeight*2 + 1; x++)
            {
                if(tiles[y][x] != null)
                {
                    try
                    {
                        if (tiles[y][x + 1] != null)
                        {
                            tiles[y][x].GetComponent<HexagonTile>().addSurroundingTile(tiles[y][x + 1]);
                        }
                    }
                    catch (System.Exception e) { }

                    try
                    {
                        if (tiles[y][x - 1] != null)
                        {
                            tiles[y][x].GetComponent<HexagonTile>().addSurroundingTile(tiles[y][x - 1]);
                        }
                    }
                    catch (System.Exception e) { }

                    if(y%2 == 1)
                    {
                        try
                        {
                            if (tiles[y + 1][x + 1] != null)
                            {
                                tiles[y][x].GetComponent<HexagonTile>().addSurroundingTile(tiles[y + 1][x + 1]);
                            }
                        }
                        catch (System.Exception e) { }

                        try
                        {
                            if (tiles[y - 1][x + 1] != null)
                            {
                                tiles[y][x].GetComponent<HexagonTile>().addSurroundingTile(tiles[y - 1][x + 1]);
                            }
                        }
                        catch (System.Exception e) { }
                    }
                    else
                    {
                        try
                        {
                            if (tiles[y + 1][x - 1] != null)
                            {
                                tiles[y][x].GetComponent<HexagonTile>().addSurroundingTile(tiles[y + 1][x - 1]);
                            }
                        }
                        catch (System.Exception e) { }

                        try
                        {
                            if (tiles[y - 1][x - 1] != null)
                            {
                                tiles[y][x].GetComponent<HexagonTile>().addSurroundingTile(tiles[y - 1][x - 1]);
                            }
                        }
                        catch (System.Exception e) { }
                    }

                    try
                    {
                        if (tiles[y + 1][x] != null)
                        {
                            tiles[y][x].GetComponent<HexagonTile>().addSurroundingTile(tiles[y + 1][x]);
                        }
                    }
                    catch (System.Exception e) { }

                    try
                    {
                        if (tiles[y - 1][x] != null)
                        {
                            tiles[y][x].GetComponent<HexagonTile>().addSurroundingTile(tiles[y - 1][x]);
                        }
                    }
                    catch (System.Exception e) { }

                    
                }
            }
        }
    }
	*/

    public void GenerateHills()
    {

    }

    //public void OnGUI()
    //{
    //    if (!SetupController.getStartGame())
    //    {
    //        if (GUI.Button(new Rect(10, 200, 125, 25), "Generate map"))
    //        {
    //            DestroyMap();
    //            GenerateMap();
    //        }
    //    }
    //}

    public void DestroyMap()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            foreach(GameObject toBeDestroy in tiles[i])
            {
                DestroyObject(toBeDestroy);
            }
        }
        tiles.Clear();
    }
}
