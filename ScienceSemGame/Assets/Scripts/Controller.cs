using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour
{
    public Node currentNode;
    public List<Node> path = new List<Node>();
    private int pathIndex = 0; // Track which node we’re moving toward

    private void Update()
    {
        CreatePath();
    }

    public void CreatePath()
    {
        if (path != null && path.Count > 0)
        {
            // Move towards current target node
            Node targetNode = path[pathIndex];

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetNode.transform.position, // use node’s actual position
                3 * Time.deltaTime
            );

            // Check if we reached the target node
            if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f)
            {
                currentNode = targetNode;
                pathIndex++;

                // If reached the end of the path, clear it
                if (pathIndex >= path.Count)
                {
                    path.Clear();
                    pathIndex = 0;
                }
            }
        }
        else
        {
            // Get all nodes in the scene
            Node[] nodes = FindObjectsOfType<Node>();

            // Generate new path if none exists
            if (nodes.Length > 0)
            {
                Node randomTarget = nodes[Random.Range(0, nodes.Length)];
                path = AStarManager.Instance.FindPath(currentNode, randomTarget);
                pathIndex = 0; // reset index
            }
        }
    }
}
