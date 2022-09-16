using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable] //Lets unity change in editor
public class HexagonTile : MonoBehaviour
{
	public string tileName;
	public GameObject tilePrefab;

	public int x; //x coord
	public int y; //y coord
	public int height = 1; // height
	public int offset = 0; //offset
	public bool isWalkable = true;
	public bool isDifficult = false;

    public Unit holdingUnit;
	private Material material;
	private Color defaultColor;
    private Color highlightColor;

	private void Awake()
	{
		material = GetComponent<MeshRenderer> ().material;
		defaultColor = material.color;
        highlightColor = material.color;
	}

    public void holdUnit(Unit unit)
    {
        holdingUnit = unit;
    }

    public void clearUnit()
    {
        holdingUnit = null;
    }

    public Unit getHoldingUnit()
    {
        return holdingUnit;
    }

    public Color getColor()
	{
		return defaultColor;
	}

	public void Highlight( Color color)
	{
		material.color = color;
	}

    public void mouseHighLight(Color color)
    {
        //Debug.Log("Setting to color " + color.ToString());
        highlightColor = material.color;
        //Debug.Log("Saving color " + highlightColor.ToString());
        material.color = color;
    }

    public void removeMouseHighLight()
    {
        //Debug.Log("Setting back to " + highlightColor.ToString());
        material.color = highlightColor;
    }

	public void removeHighlight()
	{
		material.color = defaultColor;
	}

	public List<HexagonTile> GetNeighbors()
	{
		List<HexagonTile> neighborList = new List<HexagonTile>();
		offset = y % 2; //0 on even, 1 on odd
		addTile (x - 1 + offset, y - 1, neighborList); //Top Left
		addTile (x + offset, y - 1, neighborList); //Top Right
		addTile (x - 1, y, neighborList); //Left
		addTile (x + 1, y, neighborList); //Right
		addTile (x - 1 + offset, y + 1, neighborList); //Bottom Left
		addTile (x + offset, y + 1, neighborList); //Bottom Right

		//foreach (HexagonTile item in neighborList)
		//{
		//	Debug.Log(item);
		//}

		return neighborList;
	}

	private void addTile(int x, int y, List<HexagonTile> hexagonList) //If tile exists, add it to the list
	{
		if (GameObject.Find("Hexagon" + x + "|" + y) != null)
		{
			hexagonList.Add (GameObject.Find ("Hexagon" + x + "|" + y).GetComponent<HexagonTile> ());
		}
		return;
	}

}
