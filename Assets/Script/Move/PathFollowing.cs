using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    [SerializeField] private float maxForce = 50f, maxSpeed = 50f;
    List<Vector3> pathList = new List<Vector3>();
    [SerializeField] private float pathRadius = 1.0f, futureAhead = 0.5f;

    private int currentPath = 0, transitPath = 0;
    private bool inTransitState = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    bool PointIsOnPath ( Vector3 start , Vector3 end , Vector3 point) {
        return Vector3.Dot((end - start).normalized, (point - end).normalized) < 0f && Vector3.Dot((start - end).normalized, (point - start).normalized) < 0f;
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
        if(pathList == null)
        {
            return;
        }
        for(int i =0; i  < pathList.Count - 1; i++)
        {
            Debug.DrawLine(pathList[i], pathList[i+1]);
        }
        try
        {
            Debug.DrawLine(m_Rigidbody.position, pathList[transitPath + 1], Color.cyan);
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
            Vector3 futurePosition = m_Rigidbody.position + m_Rigidbody.velocity * futureAhead;
            Debug.DrawLine(m_Rigidbody.position, futurePosition, Color.red);
            Vector3 target = FindTarget(pathList[currentPath], pathList[currentPath+1], futurePosition);
            Debug.DrawLine(m_Rigidbody.position, target, Color.green);
            float distance = Vector3.Distance(futurePosition, target);
            if(PointIsOnPath(pathList[currentPath],pathList[currentPath+1], target))
            {
                Vector3 steering = Vector3.zero;
                if(distance > pathRadius)
                {
                    steering = Seek(target, m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed);
                }
                m_Rigidbody.AddForce(steering);
                m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, maxForce);
                
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
                steering = Seek(pathList[transitPath + 1], m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed);
                m_Rigidbody.AddForce(steering);
                m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, maxForce);
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
