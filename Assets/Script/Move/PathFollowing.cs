using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    List<Vector3> pathList = new List<Vector3>();
    [SerializeField] private float maxForce =  15f, maxSpeed = 15f, slowingRadius = 0.5f, drag = 4f;
    [SerializeField] private float pathRadius = 0.00001f, futureAhead = 0.5f;
    private int currentPath = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.drag = this.drag;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPathList(List<Vector3> _pathList)
    {
        this.pathList = _pathList;
    }


    void FixedUpdate()
    {
        
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
}
