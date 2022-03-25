using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryTargetController : MonoBehaviour
{
    private int hitCount;
    private Transform[] children;

    private void Awake()
    {
        children = GetComponentsInChildren<Transform>();
    }

}
