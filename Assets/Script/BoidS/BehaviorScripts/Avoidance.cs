using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Behavior/Avoidance")]
public class Avoidance : BoidsBehavior
{
    public override Vector3 CalMove(BoidManager manager, List<Transform> context, BoidSpawnerTest BoidSpawner)
    {
        // if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector3.zero;
        }
        //add all points together and average
        Vector3 avoidanceMove = Vector3.zero;
        int count = 0;
        foreach (Transform item in context)
        {
            if (Vector3.SqrMagnitude(item.position - manager.transform.position) < BoidSpawner.SquareAvoidanceRadius)
            {
                count++;
                avoidanceMove += manager.transform.position - item.position;
            }

        }
        if(count> 0)
        {
            avoidanceMove /= count;
        }

        return avoidanceMove;
    }
}
