using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform targetPosition;

    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unWalkableMask;

    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    public List<Node> path;

    PathFinding pathFinding;

    private void Awake()
    {
        pathFinding = GetComponent<PathFinding>();
    }

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void Update()
    {
        CreateGrid();
        pathFinding.PathFind();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position 
            - Vector3.right * gridWorldSize.x / 2 
            - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft
                    + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkableMask));

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        worldPosition = RelativePosition(transform, worldPosition);

        

        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        if (percentX > 1 || percentY > 1) return null;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public Vector3 WorldPositionFromNode(Node node)
    {
        if (node == NodeFromWorldPosition(targetPosition.position)) return Vector3.zero;

        int x, y;

        if (node.gridX < gridSizeX / 2) x = -1;
        else x = 1;

        if (node.gridY < gridSizeY / 2) y = -1;
        else y = 1;

        return new Vector3(x, 1, y);
    }

    public Vector3 RelativePosition(Transform start, Vector3 target)
    {
        Vector3 distance = target - start.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, start.right.normalized);
        relativePosition.y = Vector3.Dot(distance, start.up.normalized);
        relativePosition.z = Vector3.Dot(distance, start.forward.normalized);

        return relativePosition;
    }

    public List<Node> NeighboreNodes(Node node)
    {
        List<Node> neighborNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int corX = node.gridX + x;
                int corY = node.gridY + y;

                if (corX >= 0 && corX < gridSizeX && corY >= 0 && corY < gridSizeY)
                {
                    neighborNodes.Add(grid[corX, corY]);
                }
            }
        }

        return neighborNodes;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        
        if (grid != null)
        {
            foreach (Node n in grid) {
                Gizmos.color = (n.walkable) ? Color.white : Color.yellow;
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.blue;
                    }
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
