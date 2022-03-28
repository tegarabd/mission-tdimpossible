using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;

    public Node parent;
    public Node(bool _walkable, Vector3 _worldPos, int _x, int _y)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _x;
        gridY = _y;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
