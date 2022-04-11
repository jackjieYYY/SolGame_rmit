using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Boid/Behavior/AvoidanceObstacle")]
public class AvoidanceObstacle : BoidsBehavior
{
    static Vector3[] m_ObstanceRayDirection = null;
    public AvoidanceObstacle()
    {
        setObstanceRayDirection();
    }

    public override Vector3 CalMove(BoidManager manager, List<Transform> context, BoidSpawnerTest BoidSpawner)
    {
        // if no neighbors, return no adjustment
        return CheckObstances(manager);
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


    Vector3 CheckObstances(BoidManager manager)
    {

        Vector3 bestDir = manager.transform.forward;
        int count = 0;
        float maxDis = 0;

        foreach (var dir in m_ObstanceRayDirection)
        {
            Vector3 tdir = manager.transform.TransformDirection(dir);
            var ray = new Ray(manager.transform.position, manager.transform.TransformDirection(dir));
            Debug.DrawRay(manager.transform.position, manager.transform.TransformDirection(dir), Color.blue, 1f);
            var result = Physics.RaycastAll(ray, 5f);
            if (result.Length != 0)
            {
                foreach (RaycastHit raycastHit in result)
                {
                    if (raycastHit.transform.gameObject.name.Contains("Boid"))
                    {
                        continue;
                    }
                    float dis = raycastHit.distance;
                    Debug.DrawRay(manager.transform.position, tdir, Color.red, 1f);
                    if (dis > maxDis)
                    {
                        count++;
                        bestDir = tdir;
                        maxDis = dis;
                    }
                }

            }
            else
            {
                if (count == 0)
                {
                    Debug.DrawRay(manager.transform.position, bestDir, Color.yellow, 1f);
                    return Vector3.zero;
                }

                else
                {
                    Debug.DrawRay(manager.transform.position, tdir, Color.yellow, 1f);
                    return tdir;
                }
            }
        }
        Debug.DrawRay(manager.transform.position, bestDir, Color.green, 1f);
        return Vector3.zero;
    }


}
