using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    public int id;
    public bool done;
    public string description;

    public Mission(int id, string description)
    {
        this.id = id;
        this.description = description;
        this.done = false;
    }



}
