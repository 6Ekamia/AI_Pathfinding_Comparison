using System.Collections.Generic;
using UnityEngine;

public class BFSPathfinding : MonoBehaviour
{
    public Grid grid;
    public Transform start;
    public Transform goal;
    public UnityEngine.Events.UnityEvent<Vector3[]> pathReady;

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Queue<Node> openSet = new Queue<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.Dequeue();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (closedSet.Contains(neighbor) || !neighbor.walkable)
                {
                    continue;
                }

                if (!openSet.Contains(neighbor))
                {
                    neighbor.parent = currentNode;
                    openSet.Enqueue(neighbor);
                }
            }
        }

        Debug.Log("No valid path found");
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        Vector3[] waypoints = new Vector3[path.Count];
        for (int i = 0; i < path.Count; i++)
        {
            waypoints[i] = path[i].worldPosition;
        }
        pathReady.Invoke(waypoints);
    }
}
