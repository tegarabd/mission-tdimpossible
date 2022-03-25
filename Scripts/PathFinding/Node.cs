using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gridX;
    public int gridY;

    public bool isUnwalkable;
    public Vector3 position;

    public Node Parent; // from which node this node found

    public int gCost;
    public int hCost;

    public int FCost { get { return gCost + hCost; } }

    public Node(bool isUnwalkable, Vector3 position, int gridX, int gridY)
    {
        this.isUnwalkable = isUnwalkable;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;
    }




}
