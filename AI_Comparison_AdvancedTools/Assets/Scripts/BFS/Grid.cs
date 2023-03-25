using UnityEngine;
using System.Collections.Generic;


[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distanceBetweenNodes;
    public bool showGrid = true;

    public Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        GenerateGrid();
    }

    void Start()
    {

    }

    public void GenerateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int x = node.gridX;
        int y = node.gridY;

        // Check neighbor nodes in all 8 directions
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int checkX = x + i;
                int checkY = y + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    Node neighbor = grid[checkX, checkY];
                    if (neighbor.walkable)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }
        }

        return neighbors;
    }

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            if (!Application.isPlaying) // only draw gizmos in editor mode
            {
                GenerateGrid();
            }

            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null)
            {
                foreach (Node node in grid)
                {
                    Gizmos.color = node.walkable ? Color.white : Color.red;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - distanceBetweenNodes));
                }
            }
        }
    }


}
