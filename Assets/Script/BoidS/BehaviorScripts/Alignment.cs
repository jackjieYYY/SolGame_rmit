using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Behavior/Alignment")]
public class Alignment : BoidsBehavior
{
    public override Vector3 CalMove(BoidManager manager, List<Transform> context, BoidSpawnerTest BoidSpawner)
    {
        // if no neighbors, maintain current alignment
        if (context.Count == 0)
        {
            return manager.transform.forward;
        }
        //add all points together and average
        Vector3 alignmentMove = Vector3.zero;
        foreach (Transform item in context)
        {
            alignmentMove += item.transform.forward;
        }
        alignmentMove /= context.Count;

        return alignmentMove;
    }
}
