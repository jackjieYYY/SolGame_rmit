using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boidTest : MonoBehaviour
{

    // 方向数组是静态的，只需要初始化一次，所有单元共享
    private static Vector3[] m_ObstanceRayDirection = null;

    Vector3 velocity;
    Vector3 newVelocity;
    Vector3 newPostion;



    private void Awake()
    {
        setObstanceRayDirection();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    void setObstanceRayDirection()
    {
        List<Vector3> dirs = new List<Vector3>();
        var numPoints = 180;
        var pow = 2.5f;
        var turnFraction = 0.618f;

        for (int i = 1; i < numPoints; ++i)
        {
            float dst = Mathf.Pow(i / (numPoints - 1f), pow);
            float angle = 2 * Mathf.PI * turnFraction * i;
            float x = dst * Mathf.Cos(angle);
            float y = dst * Mathf.Sin(angle);
            // only forward we need
            if (y > 0)
            {
                dirs.Add(new Vector3(x, 0, y).normalized);
            }
        }
        m_ObstanceRayDirection = dirs.ToArray();

    }


    // Update is called once per frame
    void Update()
    {
    }

    public Vector3 CheckObstances(out bool isObstances)
    {
        isObstances= false;
        Vector3 bestDir = transform.forward;
        int count = 0;
        float maxDis = 0;

        foreach (var dir in m_ObstanceRayDirection)
        {
            Vector3 tdir = transform.TransformDirection(dir);
            var ray = new Ray(transform.position, transform.TransformDirection(dir));
            Debug.DrawRay(transform.position, transform.TransformDirection(dir), Color.blue, 1f);
            var result = Physics.RaycastAll(ray, 3f);
            if (result.Length != 0)
            {
                count++;
                foreach (RaycastHit raycastHit in result)
                {
                    if(raycastHit.transform.gameObject.name == "Boid(Clone)")
                    {
                        continue;
                    }
                    float dis = raycastHit.distance;
                    Debug.DrawRay(transform.position, tdir, Color.red, 1f);
                    if (dis > maxDis)
                    {
                        bestDir = tdir;
                        maxDis = dis;
                    }
                }

            }
            else
            {
                if (count == 0)
                {
                    Debug.DrawRay(transform.position, bestDir, Color.yellow, 1f);
                    isObstances = false;
                    return bestDir;
                }
 
                else
                {
                    Debug.DrawRay(transform.position, tdir, Color.yellow, 1f);
                    isObstances = true;
                    return tdir;
                }
            }
        }
        Debug.DrawRay(transform.position, bestDir, Color.green, 1f);
        return bestDir;
    }
}
