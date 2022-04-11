using Assets.Script.Astar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    float mouseAngle;
    List<Vector3> path, path2;
    float speed = 0.01f;

    float nextFire;
    float fireRate = 0.5f;

    [SerializeField] private float maxForce =  20f, maxSpeed = 20f, slowingRadius = 0.5f, drag = 4f;
    [SerializeField] private float pathRadius = 0.001f, futureAhead = 0.25f, avoidanceDistance = 3f, avoidanceWidth = 2f;
    private int currentPath = 0;
    Vector3 lastClickPosition, steering;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.drag = this.drag;
        lastClickPosition = m_Rigidbody.position;    
    }

    private void FixedUpdate()
    {
        //The screen is oriented vertically upward is 0 degrees this.mouseAngle
        getMouseAngle();
        m_Rigidbody.rotation = Quaternion.Euler(0f, this.mouseAngle, 0f);
        if (Input.GetButton("Fire2") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            var posX = Input.mousePosition.x;
            var posY = Input.mousePosition.y;
            var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(posX, posY, 10));
            worldPos.y = 0f;
            TryGetPath(worldPos); 
            lastClickPosition = worldPos;

        }
        try
        {
            for(int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i+1], Color.cyan);
            }
            for(int i = 0; i < path2.Count - 1; i++)
            {
                Debug.DrawLine(path2[i], path2[i+1], Color.white);
            }
        }
        catch
        {

        }
        Vector3 targetDir = lastClickPosition - m_Rigidbody.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,  Quaternion.Euler(new Vector3(0f,Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg, 0f)), 0.1f);
        // steering = Seek(lastClickPosition, m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed, maxForce) + obstacleAvoidance();
        // m_Rigidbody.AddForce(steering);
        FollowPath(path);
    }



    void TryGetPath(Vector3 end)
    {
        PathManager.Request(transform.position, end, true, onPathFound);
        PathManager.Request(transform.position, end, false, onPathFound2);
        currentPath = 0;
    }

    Vector3 obstacleAvoidance()
    {
        float radius = 0.3f;
        Vector3 bottomRight = m_Rigidbody.position + (transform.right * radius) + (-transform.forward * radius);
        Vector3 bottomLeft = m_Rigidbody.position + (-transform.right * radius) + (-transform.forward * radius);
        Vector3 topRight = m_Rigidbody.position + ((transform.right * radius * avoidanceWidth) + (transform.forward * avoidanceDistance));
        Vector3 topLeft = m_Rigidbody.position + (-transform.right * radius* avoidanceWidth) + (transform.forward * avoidanceDistance);
        Debug.DrawRay(bottomRight, topRight - bottomRight, Color.green);
        Debug.DrawRay(bottomLeft, topLeft - bottomLeft, Color.green);
        Debug.DrawRay(bottomRight, bottomLeft - bottomRight, Color.green);
        Debug.DrawRay(topRight, topLeft - topRight, Color.green);
        RaycastHit[] hits = new RaycastHit[2];
        bool[] isHit = new bool[2];
        isHit[0] = Physics.Raycast(bottomLeft, topLeft - bottomLeft, out hits[0], avoidanceDistance, LayerMask.GetMask("unwalkable"));
        isHit[1] = Physics.Raycast(bottomRight, topRight - bottomRight, out hits[1], avoidanceDistance,  LayerMask.GetMask("unwalkable"));
        // Debug.DrawLine(m_Rigidbody.position, hits[0].point, Color.green);
        // Debug.DrawLine(m_Rigidbody.position, hits[1].point, Color.red);
        int leftOrRight = isHit[0] ? 0 : isHit[1] ? 1 : -1;
        if(leftOrRight != -1)
        {
            Vector3 dir = leftOrRight == 0 ? topRight : topLeft;
            Vector3 steeringToAvoid = dir + m_Rigidbody.position - hits[leftOrRight].collider.transform.position;
            steeringToAvoid *= Vector3.Distance(m_Rigidbody.position, hits[leftOrRight].collider.transform.position);
            return Seek(steeringToAvoid,m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed, maxForce);
        }
        else
        {
            return Vector3.zero;
        }
        
    }
    void onPathFound(List<Vector3> _path, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            this.path = _path;
        }
    }
    void onPathFound2(List<Vector3> _path, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            this.path2 = _path;
        }
    }
    bool PointIsOnPath(Vector3 start, Vector3 end, Vector3 point)
    {
        return Vector3.Dot((end - start).normalized, (point - end).normalized) < 0f && Vector3.Dot((start - end).normalized, (point - start).normalized) < 0f;
    }
    public static Vector3 Seek(Vector3 target, Vector3 position, Vector3 currentVelocity, float maxSpeed, float maxForce)
    {
        Vector3 desiredVelocity = (target - position).normalized * maxSpeed;
        Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - currentVelocity, maxForce);
        return steering;
    }
    public static Vector3 Arrive(Vector3 target, Vector3 position, Vector3 currentVelocity, float maxSpeed, float maxForce, float slowingRadius)
    {
        Vector3 desiredVelocity = (target - position);
        float distance = desiredVelocity.magnitude;
        if (distance < slowingRadius)
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed * (distance/slowingRadius);
        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed;
        }
        Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - currentVelocity, maxForce);
        return steering;
    }

    void FollowPath(List<Vector3> pathList)
    {

        if (pathList != null && pathList.Count != 0)
        {
            float worldRecord = 10000000000f;
            Vector3 target = Vector3.zero;
            
            Vector3 futurePosition = m_Rigidbody.position + (m_Rigidbody.velocity.normalized * futureAhead);
            for(int i = 0; i < pathList.Count-1;i++)
            {
                Vector3 start = pathList[i];
                Vector3 end = pathList[i+1];
                Vector3 normalPoint = FindTarget(start, end, futurePosition);
                if (normalPoint.z < Mathf.Min(start.z, end.z) || normalPoint.z > Mathf.Max(start.z, end.z)) 
                {
                    normalPoint.z = end.z;
                }
                if (normalPoint.x < Mathf.Min(start.x, end.x) || normalPoint.x > Mathf.Max(start.x, end.x)) 
                {
                    normalPoint.x = end.x;
                }

                float distance = Vector3.Distance(futurePosition, normalPoint);
                
                if (distance < worldRecord) 
                {
                    worldRecord = distance;
                    currentPath = i;
                    Vector3 dir = (end - start).normalized * futureAhead;
                    target = normalPoint + dir;
                }


            }
            if(worldRecord > pathRadius)
            {
                if(currentPath == pathList.Count - 2)
                {
                    m_Rigidbody.AddForce(Arrive(target, m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed, maxForce, slowingRadius));
                    currentPath = 0;  
                }
                else
                {
                    m_Rigidbody.AddForce(Seek(target, m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed, maxForce));     
                }
                m_Rigidbody.AddForce(obstacleAvoidance());
            }
            Vector3 targetDir = target - m_Rigidbody.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,  Quaternion.Euler(new Vector3(0f,Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg, 0f)), 0.1f);
        }
    }


    Vector3 FindTarget(Vector3 start, Vector3 end, Vector3 futurePostion)
    {
        Vector3 startToFuturePos = futurePostion - start;
        Vector3 pathDir = (end - start).normalized;
        startToFuturePos.y = 0f;
        pathDir.y = 0f;
        float dotProducts = Vector3.Dot(startToFuturePos, pathDir);
        pathDir *= dotProducts;
        pathDir += start;
        return pathDir;

    }


    void FollowPath()
    {
        if (path == null)
            return;
        if (path.Count == 0)
            return;
        Vector3 currentWayPoint = path[0];
        if (transform.position == currentWayPoint)
        {
            path.RemoveAt(0);
        }
    }
    void getMouseAngle()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.z = 0;
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        this.mouseAngle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
    }


    // void OnDrawGizmos()
    // {
    //     if (path != null)
    //     {
    //         for (int i = targetIndex; i < path.Count; i++)
    //         {
    //             Gizmos.color = Color.black;
    //             Gizmos.DrawCube(path[i], Vector3.one * 0.1f);
    //             if (i == targetIndex)
    //             {
    //                 Gizmos.DrawLine(transform.position, path[i]);
    //             }
    //             else
    //             {
    //                 Gizmos.DrawLine(path[i - 1], path[i]);
    //             }
    //         }
    //     }
    // }









}
