using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    public Tree lchild, rchild;
    public Container container;

    public Tree(Container container)
    {
        this.container = container;
    }
    public List<Tree> GetLeafs()
    {
        List<Tree> leafs = new List<Tree>();

        if (lchild == null && rchild == null)
        {
            leafs.Add(this);
            return leafs;
        }
        else
        {
            leafs.AddRange(lchild.GetLeafs());
            leafs.AddRange(rchild.GetLeafs());
            return leafs;
        }
    }

    public List<Tree> GetLevel(int level, List<Tree> queue)
    {
        if (queue == null)
        {
            queue = new List<Tree>();
        }
        
        if (level == 1)
        {
            queue.Add(this);
        }
        else
        {
            if (lchild != null)
            {
                lchild.GetLevel(level - 1, queue);
            }
            if (rchild != null)
            {
                rchild.GetLevel(level - 1, queue);
            }
        }
        return queue;
    }

}
