using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public int mapSize;
    public int iter;
    public float minRatio;

    Container mainContainer;
    Tree containerTree;

    GenerateAllRoom generateRoom;


    private void Start()
    {
        generateRoom = GetComponent<GenerateAllRoom>();
        mainContainer = gameObject.AddComponent<Container>();
        mainContainer.x = 0;
        mainContainer.y = 0;
        mainContainer.h = mainContainer.w = mapSize;
        containerTree = SplitContainer(mainContainer, iter);

        foreach (Tree leaf in containerTree.GetLeafs())
        {
            RandomRoom(leaf.container);
        }

        PathToAllRoom(containerTree);
    }

    public void PathToAllRoom(Tree tree)
    {
        if (tree.lchild == null || tree.rchild == null) return;
        tree.lchild.container.PathToNeighbor(tree.rchild.container);
        generateRoom.GeneratePath(tree.lchild.container, tree.rchild.container);
        PathToAllRoom(tree.lchild);
        PathToAllRoom(tree.rchild);
    }

    public Tree SplitContainer(Container container, int iter)
    {
        Tree root = new Tree(container);

        if (iter > 0)
        {
            List<Container> sepratedContainer = RandomSplit(container);
            root.lchild = SplitContainer(sepratedContainer[0], iter - 1);
            root.rchild = SplitContainer(sepratedContainer[1], iter - 1);
        }

        return root;
    }

    public List<Container> RandomSplit(Container container)
    {

        List<Container> containers = new List<Container>();
        Container c1, c2;
        float c1Ratio, c2Ratio;

        if (Random.value < 0.5f)
        {
            c1 = gameObject.AddComponent<Container>();
            c1.x = container.x;
            c1.y = Random.Range(-container.h/2, container.h/2);
            c1.w = container.w;
            c1.h = (container.h / 2 - Mathf.Abs(c1.y)) * 2;

            c2 = gameObject.AddComponent<Container>();
            c2.x = container.x;
            c2.y = (c1.y < 0) ? container.h/2 - Mathf.Abs(c1.y) : (container.h / 2 - Mathf.Abs(c1.y)) * -1;
            c2.w = container.w;
            c2.h = container.h - c1.h;

            c1.y += container.y;
            c2.y += container.y;

            c1Ratio = c1.h / c1.w;
            c2Ratio = c2.h / c2.w;

            if (c1Ratio < minRatio || c2Ratio < minRatio)
            {
                Destroy(c1);
                Destroy(c2);
                return RandomSplit(container);
            }
        }
        else
        {
            c1 = gameObject.AddComponent<Container>();
            c1.x = Random.Range(-container.w / 2, container.w / 2);
            c1.y = container.y;
            c1.w = (container.w / 2 - Mathf.Abs(c1.x)) * 2;
            c1.h = container.h;

            c2 = gameObject.AddComponent<Container>();
            c2.x = (c1.x < 0) ? container.w / 2 - Mathf.Abs(c1.x) : (container.w / 2 - Mathf.Abs(c1.x)) * -1;
            c2.y = container.y;
            c2.w = container.w - c1.w;
            c2.h = container.h;

            c1.x += container.x;
            c2.x += container.x;

            c1Ratio = c1.w / c1.h;
            c2Ratio = c2.w / c2.h;

            if (c1Ratio < minRatio || c2Ratio < minRatio)
            {
                Destroy(c1);
                Destroy(c2);
                return RandomSplit(container);
            }
        }

        

        containers.Add(c1);
        containers.Add(c2);
        return containers;
    }

    public void RandomRoom(Container container)
    {
        Room room = gameObject.AddComponent<Room>();

        room.x = container.x;
        room.y = container.y;
        room.h = container.h - 2;
        room.w = container.w - 2;

        generateRoom.GenerateRoom(room);
    }
}
