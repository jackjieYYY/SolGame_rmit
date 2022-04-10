using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawnerTest : MonoBehaviour
{
    public BoidManager boidManager;
    List<BoidManager> boidsList = new List<BoidManager>();
    public BoidsBehavior boidsBehavior;

    [Range(1, 500)]
    public int startingCount = 250;
    const float managerDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 10f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            var postion = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
            postion.y = 0;
            while (CheckPostion(postion))
            {
                postion = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
                postion.y = 0;
            }
            var boid = Instantiate(boidManager,
                postion,
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                transform
                );
            boid.name = "Boid " + i;
            
            boidsList.Add(boid);
        }
    }

    void Update()
    {
        foreach (BoidManager manager in boidsList)
        {
            List<Transform> context = GetNearByObjects(manager);

            Vector3 move = boidsBehavior.CalMove(manager, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            manager.Move(move);
        }
    }

    List<Transform> GetNearByObjects(BoidManager manager)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(manager.transform.position, neighborRadius*5);
        foreach(Collider c in contextColliders)
        {
            if(c == manager.AgentCollider)
            {
                continue;
            }
            if (c.gameObject.name.Contains("Border"))
            {
                continue;
            }
            context.Add(c.transform);
        }
        if (context.Count > 0)
        {
            var color = Color.Lerp(Color.white,Color.red,context.Count/6f);
            //Debug.DrawRay(manager.transform.position, manager.transform.TransformDirection(Vector3.forward), color, 1f);
        }
        return context;

         
    }

    bool CheckPostion(Vector3 postion)
    {
        var ray = new Ray(postion, postion+Vector3.forward);
        var result = Physics.RaycastAll(ray, 2f);
        if (result.Length > 0)
            return true;
        ray = new Ray(postion, postion + Vector3.left);
        result = Physics.RaycastAll(ray, 2f);
        if (result.Length > 0)
            return true;
        ray = new Ray(postion, postion + Vector3.right);
        result = Physics.RaycastAll(ray, 2f);
        if (result.Length > 0)
            return true;
        ray = new Ray(postion, postion + Vector3.back);
        result = Physics.RaycastAll(ray, 2f);
        if (result.Length > 0)
            return true;
        return false;
    }

}
