using Assets.Script.Astar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    float mouseAngle;
    List<Vector3> path, path2;
    int targetIndex;
    float speed = 0.01f;

    float nextFire;
    float fireRate = 0.5f;

    [SerializeField] private float maxForce =  15f, maxSpeed = 15f, slowingRadius = 0.5f, drag = 4f;
    [SerializeField] private float pathRadius = 0.00001f, futureAhead = 0.5f;
    private int currentPath = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.drag = this.drag;
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
        FollowPath(path);// this is new one with AddForce. But need to Fix.

    }



    void TryGetPath(Vector3 end)
    {
        PathManager.Request(transform.position, end, true, onPathFound);
        PathManager.Request(transform.position, end, false, onPathFound2);
        currentPath = 0;
    }

    // Vector3 obstacleAvoidance()
    // {
    //     Vector3 steering = Vector3.zero;
    //     RaycastHit hit;
    //     Vector3 target = m_Rigidbody.velocity;
    //     target.y = 0;

    //     Debug.DrawRay(m_Rigidbody.position, target * 10f,Color.green);
    //     bool rayHit = Physics.Raycast(m_Rigidbody.position, target, out hit, 1.5f, LayerMask.GetMask("unwalkable"));
    //     if (rayHit)
    //     {
    //         print(hit.distance);
    //         Vector3 diff = m_Rigidbody.position - hit.collider.gameObject.transform.position;
    //         steering += diff;
    //     }
    //     steering = steering.normalized * 5f;
    //     return steering;
    // }
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
            Vector3 futurePosition = m_Rigidbody.position + (m_Rigidbody.velocity.normalized * futureAhead);

            Vector3 start = pathList[currentPath];
            Vector3 end = pathList[currentPath+1];
            Vector3 projectedVector = FindTarget(start, end, futurePosition);
            Vector3 target = Vector3.zero;
            Vector3 futureProjected = projectedVector + ((end - start).normalized * futureAhead);

            float distance = Vector3.Distance(futurePosition, projectedVector);

            if (distance > pathRadius) 
            {
                target = futureProjected;
            }
            else 
            {
                target = futurePosition;
            }
            if (Vector2.Distance(start, projectedVector) > Vector2.Distance(start, end))
            {
                if (currentPath + 2 < pathList.Count) 
                {
                    currentPath++;

                }
                else
                {
                    target = pathList[pathList.Count-1];

                }

            }
            else
            {
                if (!PointIsOnPath(start, end, projectedVector))
                {
                    target = pathList[currentPath];
                }
            }
            Debug.DrawLine(m_Rigidbody.position, target, Color.red);
            GetComponent<MeshRenderer>().transform.rotation = Quaternion.Slerp(GetComponent<MeshRenderer>().transform.rotation,  Quaternion.Euler(target), 0.1f);
            m_Rigidbody.AddForce(Seek(target, m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed, maxForce));
        }
    }


    Vector3 FindTarget(Vector3 start, Vector3 end, Vector3 futurePostion)
    {
        Vector3 startToFuturePos = futurePostion - start;
        Vector3 pathDir = (end - start).normalized;
        float dotProducts = Vector2.Dot(new Vector2(startToFuturePos.x, startToFuturePos.z), new Vector2(pathDir.x, pathDir.z));
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
