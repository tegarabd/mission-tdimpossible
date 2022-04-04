using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public float x, y, w, h;
    private Container neighbor;

    public void PathToNeighbor(Container container)
    {
        neighbor = container;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(x, 0, y), new Vector3(w, 0.1f, h));

        /*if (neighbor)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(new Vector3(x, 2f, y), new Vector3(neighbor.x, 2f, neighbor.y));
        }*/
    }
}
