using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : MonoBehaviour
{
    private Rigidbody follower;
    [SerializeField] private GameObject Astar;
    private worldGrid grid;
    [SerializeField] private float maxForce = 50f, maxSpeed = 50f;
    List<Vector3> pathList = new List<Vector3>();
    [SerializeField] private float pathRadius = 1.0f, futureAhead = 0.5f;

    private int currentPath = 0, transitPath = 0;
    private bool inTransitState = false;
    // Start is called before the first frame update
    void Start()
    {
        follower = GetComponent<Rigidbody>();
        grid = Astar.GetComponent<worldGrid>();
        pathList.Add(new Vector3(7f,0f,5f));
        pathList.Add(new Vector3(2f,0f,3f));
        pathList.Add(new Vector3(-5f,0f,0f));
        pathList.Add(new Vector3(-6f,0f,-3f));
        pathList.Add(new Vector3(-4f,0f,-5f));
        pathList.Add(new Vector3(5f,0f,-5));
        pathList.Add(new Vector3(-5f,0f,-3f));

    }
    bool PointIsOnPath ( Vector3 start , Vector3 end , Vector3 point) {
        return Vector3.Dot((end - start).normalized, (point - end).normalized) < 0f && Vector3.Dot((start - end).normalized, (point - start).normalized) < 0f;
    }
    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        for(int i =0; i  < pathList.Count - 1; i++)
        {
            Debug.DrawLine(pathList[i], pathList[i+1]);
        }
        try
        {
            Debug.DrawLine(follower.position, pathList[transitPath + 1], Color.cyan);
        }
        catch
        {

        }

        FollowPath(pathList);
    }
    public static Vector3 Seek(Vector3 target, Vector3 position, Vector3 currentVelocity, float maxSpeed)
    {
        Vector3 desiredVelocity = (target - position).normalized * maxSpeed;
        Vector3 steering = desiredVelocity - currentVelocity;
        return steering;  
    }
    void FollowPath(List<Vector3> pathList)
    {
        if(pathList != null &&  transitPath < pathList.Count - 1)
        {
            Vector3 futurePosition = follower.position + follower.velocity * futureAhead;
            Debug.DrawLine(follower.position, futurePosition, Color.red);
            Vector3 target = FindTarget(pathList[currentPath], pathList[currentPath+1], futurePosition);
            Debug.DrawLine(follower.position, target, Color.green);
            float distance = Vector3.Distance(futurePosition, target);
            if(PointIsOnPath(pathList[currentPath],pathList[currentPath+1], target))
            {
                Vector3 steering = Vector3.zero;
                if(distance > pathRadius)
                {
                    steering = Seek(target, follower.position, follower.velocity, maxSpeed);
                }
                follower.AddForce(steering);
                follower.velocity = Vector3.ClampMagnitude(follower.velocity, maxForce);
                
            }        
            else if (!PointIsOnPath(pathList[currentPath],pathList[currentPath+1], target) && transitPath < pathList.Count - 1)
            {   
                transitPath = !inTransitState ? transitPath + 1 : transitPath;
                if(transitPath >= pathList.Count - 1)
                {
                    return;
                }
                inTransitState = true;
                Vector3 steering = Vector3.zero;
                steering = Seek(pathList[transitPath + 1], follower.position, follower.velocity, maxSpeed); 
                follower.AddForce(steering);
                follower.velocity = Vector3.ClampMagnitude(follower.velocity, maxForce);
                Vector3 newTarget = FindTarget(pathList[transitPath], pathList[transitPath+1], futurePosition);
                if(PointIsOnPath(pathList[transitPath],pathList[transitPath+1], newTarget))
                {
                    inTransitState  = false;
                    currentPath++;
                }
            }
        }
    }
    Vector3 FindTarget(Vector3 start, Vector3 end, Vector3 futurePostion) 
    {
        Vector3 v1 = futurePostion - start;
        Vector3 v2 = (end - start).normalized;
        float dotProducts = Vector2.Dot(new Vector2(v1.x, v1.z), new Vector2(v2.x, v2.z));
        v2 *= dotProducts;
        v2 += start;
        return v2;
    }
}
