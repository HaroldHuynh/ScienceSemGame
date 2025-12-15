using UnityEngine;
using System.Collections.Generic;

public class AStarManager : MonoBehaviour
{
    public static AStarManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public List<Node> FindPath(Node startNode, Node endNode)
    {
        List<Node> openSet = new List<Node>();

        // Reset gCosts
        foreach (Node node in FindObjectsOfType<Node>())
        {
            node.gCost = float.MaxValue;
        }

        startNode.gCost = 0;
        startNode.hCost = Vector3.Distance(startNode.transform.position, endNode.transform.position);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            int lowestF = 0;

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() < openSet[lowestF].fCost())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.RemoveAt(lowestF);

            if (currentNode == endNode)
            {
                List<Node> path = new List<Node>();

                path.Insert(0, endNode);

                while (currentNode != startNode)
                {
                    currentNode = currentNode.cameFrom;
                    path.Insert(0, currentNode);
                }

                return path;
            }

            foreach (Node connectedNode in currentNode.connections)
            {
                float heldgCost = currentNode.gCost +
                                  Vector3.Distance(currentNode.transform.position,
                                                   connectedNode.transform.position);

                if (heldgCost < connectedNode.gCost)
                {
                    connectedNode.cameFrom = currentNode;
                    connectedNode.gCost = heldgCost;
                    connectedNode.hCost =
                        Vector3.Distance(connectedNode.transform.position, endNode.transform.position);

                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        return null;
    }
}
