using Assets.Script.Astar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    float mouseAngle;
    List<Vector3> path;
    int targetIndex;
    float speed = 0.01f;

    float nextFile;
    float fileRate = 0.5f;

    [SerializeField] private float maxForce = 50f, maxSpeed = 50f;
    [SerializeField] private float pathRadius = 1.0f, futureAhead = 0.5f;
    private int currentPath = 0, transitPath = 0;
    private bool inTransitState = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //The screen is oriented vertically upward is 0 degrees this.mouseAngle
        getMouseAngle();
        m_Rigidbody.rotation = Quaternion.Euler(0f, this.mouseAngle, 0f);

        if (Input.GetButton("Fire2") && Time.time > nextFile)
        {
            nextFile = Time.time + fileRate;
            var posX = Input.mousePosition.x;
            var posY = Input.mousePosition.y;
            var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(posX, posY, 10));
            worldPos.y = 0f;
            TryGetPath(worldPos);
        }

        //FollowPath();// this is  original method.
        FollowPath();// this is new one with AddForce. But need to Fix.
    }



    void TryGetPath(Vector3 end)
    {
        PathManager.Request(transform.position, end, onPathFound);
    }


    void onPathFound(List<Vector3> _path, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            this.path = _path;
        }
    }
    bool PointIsOnPath(Vector3 start, Vector3 end, Vector3 point)
    {
        return Vector3.Dot((end - start).normalized, (point - end).normalized) < 0f && Vector3.Dot((start - end).normalized, (point - start).normalized) < 0f;
    }
    public static Vector3 Seek(Vector3 target, Vector3 position, Vector3 currentVelocity, float maxSpeed)
    {
        Vector3 desiredVelocity = (target - position).normalized * maxSpeed;
        Vector3 steering = desiredVelocity - currentVelocity;
        return steering;
    }

    void FollowPath(List<Vector3> pathList)
    {
        if (pathList != null && transitPath < pathList.Count - 1)
        {
            Vector3 futurePosition = m_Rigidbody.position + m_Rigidbody.velocity * futureAhead;
            Debug.DrawLine(m_Rigidbody.position, futurePosition, Color.red);
            Vector3 target = FindTarget(pathList[currentPath], pathList[currentPath + 1], futurePosition);
            Debug.DrawLine(m_Rigidbody.position, target, Color.green);
            float distance = Vector3.Distance(futurePosition, target);
            if (PointIsOnPath(pathList[currentPath], pathList[currentPath + 1], target))
            {
                Vector3 steering = Vector3.zero;
                if (distance > pathRadius)
                {
                    steering = Seek(target, m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed);
                }
                m_Rigidbody.AddForce(steering);
                m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, maxForce);

            }
            else if (!PointIsOnPath(pathList[currentPath], pathList[currentPath + 1], target) && transitPath < pathList.Count - 1)
            {
                transitPath = !inTransitState ? transitPath + 1 : transitPath;
                if (transitPath >= pathList.Count - 1)
                {
                    return;
                }
                inTransitState = true;
                Vector3 steering = Vector3.zero;
                steering = Seek(pathList[transitPath + 1], m_Rigidbody.position, m_Rigidbody.velocity, maxSpeed);
                m_Rigidbody.AddForce(steering);
                m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, maxForce);
                Vector3 newTarget = FindTarget(pathList[transitPath], pathList[transitPath + 1], futurePosition);
                if (PointIsOnPath(pathList[transitPath], pathList[transitPath + 1], newTarget))
                {
                    inTransitState = false;
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




    void FollowPath()
    {
        if (path == null)
            return;
        if (path.Count == 0)
            return;
        Vector3 currentWayPoint = path[0];
        transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed);
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
    void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Count; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.1f);
                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }









}
