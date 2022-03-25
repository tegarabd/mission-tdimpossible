using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    public Transform StartPosition;
    public Transform TargetPosition;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);
    }

    private void FindPath(Vector3 StartPosition, Vector3 TargetPostion)
    {
        Node StartNode = grid.NodeFromWorldPosition(StartPosition);
        Node TargetNode = grid.NodeFromWorldPosition(TargetPostion);

        List<Node> Open = new List<Node>();
        HashSet<Node> Close = new HashSet<Node>();

        Open.Add(StartNode);

        while (Open.Count > 0)
        {
            Node Current = Open[0];

            for (int i = 0; i < Open.Count; i++)
            {
                if (Open[i].FCost <= Current.FCost && Open[i].hCost < Current.hCost)
                {
                    Current = Open[i];
                }

                Open.Remove(Current);
                Close.Add(Current);

                if (Current == TargetNode)
                {
                    GetFinalPath(StartNode, TargetNode);
                }

                foreach (Node Neighbor in grid.GetNeighborNodes(Current))
                {
                    if (!Neighbor.isUnwalkable || Close.Contains(Neighbor))
                    {
                        continue;
                    }

                    int MoveCost = Current.gCost + GetManhattenDistance(Current, Neighbor);

                    if (MoveCost < Neighbor.gCost || !Open.Contains(Neighbor))
                    {
                        Neighbor.gCost = MoveCost;
                        Neighbor.hCost = GetManhattenDistance(Neighbor, TargetNode);
                        Neighbor.Parent = Current;

                        if (!Open.Contains(Neighbor))
                        {
                            Open.Add(Neighbor);
                        }
                    }
                }
            }
        }
    }

    private void GetFinalPath(Node start, Node target)
    {
        List<Node> FinalPath = new List<Node>();
        Node Current = target;

        while (Current != start)
        {
            FinalPath.Add(Current);
            Current = Current.Parent;
        }

        FinalPath.Reverse();

        grid.FinalPath = FinalPath;
    }

    private int GetManhattenDistance(Node node, Node neighbor)
    {
        int ix = Mathf.Abs(node.gridX - neighbor.gridX);
        int iy = Mathf.Abs(node.gridY - neighbor.gridY);

        return ix + iy;
    }
}
