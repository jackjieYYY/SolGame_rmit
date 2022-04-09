using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boidTest : MonoBehaviour
{

    // 方向数组是静态的，只需要初始化一次，所有单元共享
    private static Vector3[] m_ObstanceRayDirection = null;

    private void Awake()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void setObstanceRayDirection()
    {
        List<Vector3> dirs = new List<Vector3>();
        // 这里使用120个点；实际是60个，因为要去掉朝后的射线。
        for (int i = 1; i < 120; ++i)
        {
            float t = i / 119f; // 120 - 1
            float inc = Mathf.Acos(1f - 2f * t);
            float z = Mathf.Cos(inc);
            if (z > 0) // 只关心朝前的方向，忽略朝后的方向
            {
                float az = 2f * Mathf.PI * 1.618f * i; // 黄金分割率+1
                float x = Mathf.Sin(inc) * Mathf.Cos(az);
                float y = Mathf.Sin(inc) * Mathf.Sin(az);
                dirs.Add(new Vector3(x, y, z).normalized);
            }
        }
        m_ObstanceRayDirection = dirs.ToArray();
    }


    // Update is called once per frame
    void Update()
    {
        setObstanceRayDirection();
        CheckObstances();

    }

    private Vector3 CheckObstances()
    {
        Vector3 bestDir = transform.forward;
        float maxDis = 0;
        foreach (var dir in m_ObstanceRayDirection)
        {
            Vector3 tdir = transform.TransformDirection(dir);
            Debug.DrawRay(transform.position, tdir, Color.red, 1f);
            var ray = new Ray(transform.position, transform.TransformDirection(dir));
            var result = Physics.RaycastAll(ray, 10f);
            foreach (RaycastHit raycastHit in result)
            {
                float dis = raycastHit.distance;
                if (dis > maxDis)
                {
                    bestDir = tdir;
                    maxDis = dis;
                }
                return tdir;
            }   
        }
        return bestDir;
    }

}
