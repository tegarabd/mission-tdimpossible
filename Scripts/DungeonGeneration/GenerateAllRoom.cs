using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAllRoom : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefabs;
    private GameObject roomParent, pathParent;
    private float offset = 2f;
    public void GenerateRoom(Room room)
    {
        roomParent = new GameObject("Room");
        roomParent.transform.SetParent(transform);
        roomParent.transform.position = new Vector3(room.x, 0f, room.y);

        List<GameObject> walls = new List<GameObject>()
        {
            Instantiate(wallPrefabs, new Vector3(room.x - room.w / 2, 1.5f, room.y), Quaternion.identity, roomParent.transform),
            Instantiate(wallPrefabs, new Vector3(room.x + room.w / 2, 1.5f, room.y), Quaternion.identity, roomParent.transform),
            Instantiate(wallPrefabs, new Vector3(room.x, 1.5f, room.y - room.h / 2), Quaternion.Euler(0, 90f, 0), roomParent.transform),
            Instantiate(wallPrefabs, new Vector3(room.x, 1.5f, room.y + room.h / 2), Quaternion.Euler(0, 90f, 0), roomParent.transform)
        };

        

        foreach (GameObject wall in walls)
        {
            if (wall.transform.rotation == Quaternion.Euler(0, 90f, 0))
            {
                wall.transform.localScale = new Vector3(wall.transform.localScale.x * 2, 3, room.w + offset);
            }
            else
            {
                wall.transform.localScale = new Vector3(wall.transform.localScale.x * 2, 3, room.h + offset);
            }
        }
        
    }

    public void GeneratePath(Container c1, Container c2)
    {
        pathParent = new GameObject("Path");
        pathParent.transform.SetParent(transform);
        pathParent.transform.position = Vector3.Lerp(new Vector3(c1.x, 1.5f, c1.y), new Vector3(c2.x, 1.5f, c2.y), 0.5f);

        

        if (c1.x == c2.x)
        {
            GameObject wall = Instantiate(wallPrefabs, new Vector3(pathParent.transform.position.x - offset, pathParent.transform.position.y, pathParent.transform.position.z), Quaternion.identity, pathParent.transform);
            wall.transform.localScale = new Vector3(2f, 3f, Mathf.Abs(c2.y - c1.y));
            GameObject wall1 = Instantiate(wallPrefabs, new Vector3(pathParent.transform.position.x + offset, pathParent.transform.position.y, pathParent.transform.position.z), Quaternion.identity, pathParent.transform);
            wall1.transform.localScale = new Vector3(2f, 3f, Mathf.Abs(c2.y - c1.y));
        }
        else if (c1.y == c2.y)
        {
            GameObject wall = Instantiate(wallPrefabs, new Vector3(pathParent.transform.position.x, pathParent.transform.position.y, pathParent.transform.position.z - offset), Quaternion.Euler(0, 90f, 0), pathParent.transform);
            wall.transform.localScale = new Vector3(2f, 3f, Mathf.Abs(c2.x - c1.x));
            GameObject wall1 = Instantiate(wallPrefabs, new Vector3(pathParent.transform.position.x, pathParent.transform.position.y, pathParent.transform.position.z + offset), Quaternion.Euler(0, 90f, 0), pathParent.transform);
            wall1.transform.localScale = new Vector3(2f, 3f, Mathf.Abs(c2.x - c1.x));
        }
        
    }
}
