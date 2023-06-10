using System;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector2Int gridPosition;
    public int gCost = 0; // рассто€ние от начального узла
    public int hCost = 0; // рассто€ние от конечного узла
    public Node parentNode;

    public Node(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;

        parentNode = null;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        // значение compare будет <0, если стоимость этого экземпл€ра меньше, чем у nodeToCompare.FCost
        // значение сравнени€ будет >0, если стоимость этого экземпл€ра больше, чем nodeToCompare.FCost
        // сравнение будет ==0, если значени€ совпадают

        int compare = FCost.CompareTo(nodeToCompare.FCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return compare;
    }
}
