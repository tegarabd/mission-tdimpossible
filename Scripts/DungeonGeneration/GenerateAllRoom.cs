using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAllRoom : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefabs;
    private GameObject roomParent, pathParent;
    private float offset = 2f;

    public static bool[,] map = new bool[100,100];

    public void GeneratePathToExit(Container container)
    {
        int x = Mathf.RoundToInt(container.x);
        int y = Mathf.RoundToInt(container.y);

        for (int i = x; i < 100; i++)
        {
            if (y + 1 <= 99) map[i, y + 1] = true;
            if (y - 1 >= 0) map[i, y - 1] = true;
            map[i, y] = true;
        }

    }
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

        int x1 = Mathf.RoundToInt(c1.x);
        int x2 = Mathf.RoundToInt(c2.x);
        int y1 = Mathf.RoundToInt(c1.y);
        int y2 = Mathf.RoundToInt(c2.y);
        int temp;

        if (x2 < x1)
        {
            temp = x1;
            x1 = x2;
            x2 = temp;
        }

        if (y2 < y1)
        {
            temp = y1;
            y1 = y2;
            y2 = temp;
        }

        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                if (x - 1 > 0 && y -1  > 0 && x + 1 < 99 && y + 1< 99)
                {
                    map[x, y] = true;
                }
                if (x1 == x2 && x - 1 > 0 && x + 1 < 99)
                {
                    map[x - 1, y] = true;
                    map[x + 1, y] = true;
                }
                else if (y1 == y2 && y - 1 > 0 && y + 1 < 99)
                {
                    map[x, y - 1] = true;
                    map[x, y + 1] = true;
                }
            }
        }
    }
}
