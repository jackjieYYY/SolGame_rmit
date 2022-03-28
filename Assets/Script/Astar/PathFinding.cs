using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    private int NORMAL_DIAGONAL_COST = 14, NORMAL_STRAIGHT_COST = 10;
    public Transform seeker, target;
    worldGrid grid;

    private void Awake()
    {
        grid = GetComponent<worldGrid>();
    }

    public void findPath(Vector3 startPos, Vector3 targetPos)
    {

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        Debug.DrawLine(startNode.worldPostion, targetNode.worldPostion, Color.red);
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fcost < currentNode.fcost || openSet[i].fcost == currentNode.fcost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (isNeighbour(currentNode, targetNode))
            {
                RetracePath(startNode, currentNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                int newMovementCostToNeighbour = currentNode.gCost +  GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour,targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }


    bool isNeighbour(Node currentNode,Node targetNode)
    {
        var Neightbour = grid.GetNeighbours(targetNode);
        return Neightbour.Exists(x=>x == currentNode);
    }


    void RetracePath(Node startNode,Node endNode)
    {

        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        grid.path = path;
    }


    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return NORMAL_DIAGONAL_COST * Mathf.Min(dstX, dstY) + NORMAL_STRAIGHT_COST * Mathf.Abs(dstY - dstX);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //https://docs.unity3d.com/ScriptReference/Physics.RaycastAll.html
        // Creat a ray to Find the closest point to seeker
        var ray = new Ray(seeker.position, target.position - seeker.position);
        var result = Physics.RaycastAll(ray, float.MaxValue);
        foreach (RaycastHit rayCastHit in result)
        {
            if(rayCastHit.transform == target)
            {
                var HitPoint = rayCastHit.point;
                findPath(seeker.position, HitPoint);
            }
        }
    }
}
