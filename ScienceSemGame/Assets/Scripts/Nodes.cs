using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections;

    public float gCost; // Cost from start node to this node
    public float hCost; // Heuristic cost to the end node   

    public float fCost()
    {
        return gCost + hCost;
    }
}
