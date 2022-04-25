using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPathFinding : MonoBehaviour
{
    public Transform enemy;
    private Node randomNode;

    BossGrid grid;

    private void Awake()
    {
        grid = GetComponent<BossGrid>();
    }

    public void SetRandomNode()
    {
        randomNode = grid.grid[Mathf.RoundToInt(Random.Range(0, grid.gridSizeX)), Mathf.RoundToInt(Random.Range(0, grid.gridSizeY))];
    }

    public void PathFind()
    {
        Node startNode = grid.NodeFromWorldPosition(enemy.position);
        Node targetNode = randomNode;

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            // get lowest f cost
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in grid.NeighboreNodes(currentNode))
            {
                /*if (neighbor == null) Debug.Log("neighbor null");
                if (targetNode == null) Debug.Log("target node null");*/
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node targetNode)
    {
        grid.path.Clear();
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        /*if (nodeA == null) Debug.Log("A null");
        if (nodeB == null) Debug.Log("B null");*/

        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }
}
