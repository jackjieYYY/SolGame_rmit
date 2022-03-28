using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPostion;

    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;
    public Node Parent;

    public int heapIndex;


    public Node(bool _walkable,Vector3 _worldPostion,int _gridX,int _gridY)
    {
        walkable = _walkable;
        worldPostion = _worldPostion;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fcost
    {
        get { return gCost+hCost; }
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node other)
    {
        int compare  = fcost.CompareTo(other.fcost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return compare;

    }
}
