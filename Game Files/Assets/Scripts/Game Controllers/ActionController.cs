using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class ActionController : MonoBehaviour 
{
    //gets distance between two tiles, used for heuristic
	public static float getDistance(HexagonTile tileOne, HexagonTile tileTwo)
	{
		Vector3 tileOnePosition = tileOne.transform.position;
		Vector3 tileTwoPosition = tileTwo.transform.position;
		float xDif = Mathf.Abs(tileOnePosition.x - tileTwoPosition.x);
		float zDif = Mathf.Abs (tileOnePosition.z - tileTwoPosition.z);
		return Mathf.Sqrt( Mathf.Pow(xDif,2) + Mathf.Pow(zDif,2) );
	}


    //Converts A List of Nodes to a List of Hexagons
    public static List<HexagonTile> NodeToHexagon(List<Node> nodeList)
    {
        List<HexagonTile> hexagonList = new List<HexagonTile>();
        foreach (Node node in nodeList)
            hexagonList.Add(node.hexagonTile);
        return hexagonList;
    }

    //Returns possible successors with height constraint
    public static List<HexagonTile> getSuccessors(HexagonTile currentTile, int maximumHeightDifference = 1)
    {
        List<HexagonTile> successors = new List<HexagonTile>();

        //The X, Y Coordinates for the current tile
        int x = currentTile.x;
        int y = currentTile.y;
        int offset = y % 2; //0 on even, 1 on odd

        //The X, Y coordinates to check for successors
        int[,] sucCoord = new int[6, 2]
        {
            { x - 1 + offset, y - 1 },
            { x + offset, y - 1 },
            { x - 1, y },
            { x + 1, y },
            {x - 1 + offset, y + 1},
            {x + offset, y + 1}
        };


        HexagonTile nextTile;
        for (int index = 0; index < 6; index++)
        {
            if (GameObject.Find("Hexagon" + sucCoord[index, 0] + "|" + sucCoord[index, 1]) != null)
            {
                nextTile = MapController.hexagonTiles[sucCoord[index, 0], sucCoord[index, 1]];
                if (nextTile != null) successors.Add(nextTile);
            }
        }
        return successors;
    }


    //A* movement for pathfinding
    public static List<HexagonTile> unitPathfinding(HexagonTile startingTile, HexagonTile destinationTile, int maximumHeightDifference = 1)
	{
		List<Node> visitted = new List<Node> ();
		Node currentNode = new Node (null, startingTile); 

		SimplePriorityQueue<Node> fringe = new SimplePriorityQueue<Node> ();

		fringe.Enqueue(currentNode, currentNode.pathCost);
		while (fringe.Count > 0) 
		{
			currentNode = fringe.Dequeue();
			if (!visitted.Contains (currentNode)) 
			{
				visitted.Add (currentNode);

				if (currentNode.hexagonTile == destinationTile)
					return NodeToHexagon(currentNode.nodePath());

				foreach (Node node in currentNode.expand()) 
				{
					if (!visitted.Contains (node)) 
					{
						float distance = getDistance (startingTile, destinationTile);
						if (node.hexagonTile.isWalkable && Mathf.Abs (node.hexagonTile.height - node.parent.hexagonTile.height) <= maximumHeightDifference)
							fringe.Enqueue (node, node.pathCost + distance);	
					}
				}
			}
		}
		return null;
	}

    //Projectile pathfinding
    public static List<HexagonTile> projectilePathfinding(HexagonTile startingTile, HexagonTile destinationTile, int maximumHeightDifference = 1)
    {
        List<Node> visitted = new List<Node>();
        Node currentNode = new Node(null, startingTile);

        SimplePriorityQueue<Node> fringe = new SimplePriorityQueue<Node>();

        fringe.Enqueue(currentNode, currentNode.pathCost);
        while (fringe.Count > 0)
        {
            currentNode = fringe.Dequeue();
            if (!visitted.Contains(currentNode))
            {
                visitted.Add(currentNode);

                if (currentNode.hexagonTile == destinationTile)
                    return NodeToHexagon(currentNode.nodePath());

                foreach (Node node in currentNode.expand())
                {
                    if (!visitted.Contains(node))
                    {
                        float distance = getDistance(startingTile, destinationTile);
                        if (Mathf.Abs(node.hexagonTile.height - node.parent.hexagonTile.height) <= maximumHeightDifference)
                            fringe.Enqueue(node, node.pathCost + distance);
                    }
                }
            }
        }
        return null;
    }
    //Returns all tiles in given range and height
    public static List<HexagonTile> findMovable(HexagonTile startingTile, int maximumDepth = 3, int maximumHeightDifference = 1)
	{
		List<Node> visitted = new List<Node> ();
		Node currentNode = new Node (null, startingTile); 

		SimplePriorityQueue<Node> fringe = new SimplePriorityQueue<Node> ();

		fringe.Enqueue(currentNode, currentNode.pathCost);
		while (fringe.Count > 0) 
		{
			currentNode = fringe.Dequeue();
			if (!visitted.Contains (currentNode)) 
			{
                if (currentNode.depth > maximumDepth) return NodeToHexagon(visitted);
                visitted.Add (currentNode);

				foreach (Node node in currentNode.expand()) 
				{
					if (!visitted.Contains (node)) 
					{
						if (node.hexagonTile.isWalkable && Mathf.Abs (node.hexagonTile.height - node.parent.hexagonTile.height) <= maximumHeightDifference)
							fringe.Enqueue (node, node.pathCost);	
					}
				}
			}
		}
		return null;
	}

    public static List<HexagonTile> findAttackable(HexagonTile startingTile, int maximumDepth = 3, int maximumHeightDifference = 4)
    {
        List<Node> visitted = new List<Node>();
        Node currentNode = new Node(null, startingTile);

        SimplePriorityQueue<Node> fringe = new SimplePriorityQueue<Node>();

        fringe.Enqueue(currentNode, currentNode.pathCost);
        while (fringe.Count > 0)
        {
            currentNode = fringe.Dequeue();
            if (!visitted.Contains(currentNode))
            {
                if (currentNode.depth > maximumDepth) return NodeToHexagon(visitted);
                visitted.Add(currentNode);

                foreach (Node node in currentNode.expand())
                {
                    if (!visitted.Contains(node))
                    {
                        if (Mathf.Abs(node.hexagonTile.height - node.parent.hexagonTile.height) <= maximumHeightDifference)
                            fringe.Enqueue(node, node.pathCost);
                    }
                }
            }
        }
        return null;
    }


    /*
	public static void highlightRange(HexagonTile hexagonTile, int tileRange = 4, int maximumHeightDifference = 1)
	{
		foreach (Node node in findRanges(hexagonTile, tileRange))
		{
			node.hexagonTile.Highlight(Color.cyan);		
		}
		return;
	}


	public static void removeHighlightRange(HexagonTile hexagonTile, float tileRange = 10, int maximumHeightDifference = 1)
	{
		foreach (HexagonTile tile in tilesInRange(hexagonTile, tileRange, maximumHeightDifference)) 
		{
			tile.removeHighlight ();
		}
		return;
	}
    */

    public static HexagonTile getTile(int x, int y)
	{
		return MapController.hexagonTiles [x, y];
	}
	public static HexagonTile[,] getTileMap()
	{
		return MapController.hexagonTiles;
	}
}
