using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float x, y, w, h;
    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.gray;
        Gizmos.DrawCube(new Vector3(x, 0, y), new Vector3(w, 2f, h));*/
    }
}
