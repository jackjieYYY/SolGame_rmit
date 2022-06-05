using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldGrid : MonoBehaviour
{
    public Transform player;

    public bool displayGridGizmos;

    public LayerMask unwalkableMask, asteroidFieldMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Node[,] myGrid;

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (myGrid != null && displayGridGizmos)
        {
            foreach (Node n in myGrid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPostion, Vector3.one * (nodeDiameter - 0.1f));
            }
        }

    }


    // Start is called before the first frame update
    void Awake()
    {
        unwalkableMask = LayerMask.GetMask("unwalkable");
        asteroidFieldMask = LayerMask.GetMask("asteroidField");
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public Node NodeFromWorldPoint(Vector3 worldPostion)
    {
        float percentX = (worldPostion.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPostion.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return myGrid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node> ();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(myGrid[checkX,checkY]);
                }
            }
        }
        return neighbours;
    }

    void CreateGrid()
    {
        myGrid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottemLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottemLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
                myGrid[x, y] = new Node(walkable, worldPoint,x,y);
                if(Physics.CheckSphere(worldPoint, nodeRadius, asteroidFieldMask))
                {
                    myGrid[x, y].costMultiplier *= 2;
                }
                else if(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask))
                {
                    myGrid[x, y].costMultiplier *= 3;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CreateGrid();
    }
}
