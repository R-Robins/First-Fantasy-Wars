using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Node
{
	public Node parent;
	public int pathCost = 0, depth = 0;
	public HexagonTile hexagonTile;

	void Start()
	{
		ActionController.getSuccessors ( hexagonTile);
	}

	public Node(Node parent = null, HexagonTile tile = null, int pathCost = 0)
	{
		this.parent = parent;
		this.hexagonTile = tile;

		if (parent != null) 
		{
			this.pathCost = parent.pathCost + pathCost;
			this.depth = parent.depth + 1;
		} 
		else 
		{
			this.pathCost = pathCost;
			this.depth = 0;
		}
	}

	public List<Node> nodePath()
	{
		Node currentNode = this;
		List <Node> resultList = new List <Node>();
		resultList.Add(currentNode);
		while (currentNode.parent != null)
		{
			resultList.Add (currentNode.parent);
			currentNode = currentNode.parent;
		}
		resultList.Reverse ();
		return resultList;
	}

	public List<Node> expand()
	{
		List<Node> expansions = new List<Node>();
		foreach (HexagonTile tile in ActionController.getSuccessors(hexagonTile)) {
			expansions.Add (new Node (this, tile, 1));
		}
		return expansions;
	}
}
