using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform StartPosition;
    public LayerMask UnWalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;

    Node[,] grid;
    public List<Node> FinalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        
        for (int i = 0; i < gridSizeY; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                Vector3 worldPoint = bottomLeft
                    + Vector3.right * (j * nodeDiameter + nodeRadius)
                    + Vector3.forward * (i * nodeDiameter + nodeRadius);
                bool UnWalkable = true;

                if (Physics.CheckSphere(worldPoint, nodeRadius, UnWalkableMask))
                {
                    UnWalkable = false;
                }

                grid[j, i] = new Node(UnWalkable, worldPoint, i, j);
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float xPoint = ((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
    }

    public List<Node> GetNeighborNodes(Node node)
    {
        List<Node> NeighborNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    NeighborNodes.Add(grid[checkX, checkY]);
                }

            }
        }

        return NeighborNodes;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                if (node.isUnwalkable)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }

                if (FinalPath != null)
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - Distance));
            }
        }
    }
}
