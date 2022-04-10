using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Behavior/SteeredCohesion")]
public class SteeredCohesion : BoidsBehavior
{

    Vector3 currentVelocity;
    public float SmoothTime = 0.5f;


    public override Vector3 CalMove(BoidManager manager, List<Transform> context, BoidSpawnerTest BoidSpawner)
    {
        // if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector3.zero;
        }
        //add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        foreach (Transform item in context)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        cohesionMove -= manager.transform.position;
        cohesionMove = Vector3.SmoothDamp(manager.transform.forward, cohesionMove,ref currentVelocity, SmoothTime);
        return cohesionMove;
    }
}
