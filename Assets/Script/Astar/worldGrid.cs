using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class worldGrid : MonoBehaviour
{
    public Transform player;


    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Node[,] myGrid;
    [SerializeField] private bool YellowPathOnly = true;

    public List<Node> path;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (myGrid != null)
        {
            Node playerNode = null;
            try
            {
                playerNode = NodeFromWorldPoint(player.position);
            }
            catch
            {
                //Debug.Log("Player die!");
            }
            foreach (Node n in myGrid)
            {
                if (playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                else if (n.walkable)
                {
                    Gizmos.color = Color.white;

                }
                else if(!n.walkable)
                {
                    Gizmos.color = Color.red;
                }
                if(path != null)
                {
                    if (path.Exists(x=>x.gridX == n.gridX && x.gridY == n.gridY))
                    {
                        Gizmos.color = Color.yellow;
                    }
                    // GetSmootherPath();

                }
                Gizmos.DrawCube(n.worldPostion, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
    public List<Vector3> GetSmootherPath()
    {
        if (path != null)
        {
            //Make a Vec3 list with player vec3 in it
            List<Vector3> pathInVec3 = new List<Vector3>();
            Node playerNode = NodeFromWorldPoint(player.position);
            pathInVec3.Add(playerNode.worldPostion);
            foreach (Node n in path)
            {
                pathInVec3.Add(n.worldPostion);
            }

            //Draw debug lines for pathfinding before smoothing
            for(int i = 0; i < pathInVec3.Count-1; i++)
            {
                Debug.DrawLine(pathInVec3[i], pathInVec3[i+1]);
            }

            //Path smoothing
            //Create a new list for smoother path
            List<Vector3> pathSmoothing = new List<Vector3>();
            pathSmoothing.Add(pathInVec3[0]);
            
            //------
            int currIdx = 0;
            for(int i = 0; i < pathInVec3.Count-1; i++)
            {
                if(i == currIdx)
                {
                    for(int j = i + 1; j < pathInVec3.Count; j++)
                    {
                        if(YellowPathOnly)
                        {
                            Vector3 direction = (pathInVec3[j] - pathInVec3[i]).normalized * (pathInVec3[j] - pathInVec3[i]).magnitude;
                            bool hit = Physics.Raycast(pathInVec3[i], direction, (pathInVec3[j] - pathInVec3[i]).magnitude, unwalkableMask);
                            if(hit)
                            {                     
                                pathSmoothing.Add(pathInVec3[j-1]);
                                currIdx = j -1;
                                break;                                      
                            }
                            else
                            {
                                for(float k = 0; k < (pathInVec3[j] - pathInVec3[i]).magnitude; k += 0.01f)
                                {
                                    Vector3 P = Vector3.Lerp(pathInVec3[i], pathInVec3[j], k);
                                    Vector3 n = new Vector3(Mathf.Round(P.x), P.y, Mathf.Round(P.z));
                                    if(Physics.CheckSphere(n, nodeRadius,unwalkableMask))
                                    {
                                        pathSmoothing.Add(pathInVec3[j-1]);
                                        currIdx = j -1;
                                        break;
                                    }
                                }
                            }                            
                        }
                        else
                        {
                            Vector3 direction = (pathInVec3[j] - pathInVec3[i]).normalized * (pathInVec3[j] - pathInVec3[i]).magnitude;
                            RaycastHit rayHit;
                            bool hit = Physics.SphereCast(pathInVec3[i], nodeRadius, direction, out rayHit, (pathInVec3[j] - pathInVec3[i]).magnitude, unwalkableMask);
                            if(hit)
                            {                     
                                pathSmoothing.Add(pathInVec3[j-1]);
                                currIdx = j -1;
                                break;                                      
                            }
                        }
                    }
                }
            }  
            pathSmoothing.Add(pathInVec3[pathInVec3.Count-1]);
            //------
            //Draw debug lines for path after smoothing
            for(int i = 0; i < pathSmoothing.Count-1; i++)
            {
                Debug.DrawLine(pathSmoothing[i], pathSmoothing[i+1], Color.cyan);
            }
            return pathSmoothing;
        }
        return null;
    }
    
    // Start is called before the first frame update
    void Start()
    {
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
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CreateGrid();
    }
}
