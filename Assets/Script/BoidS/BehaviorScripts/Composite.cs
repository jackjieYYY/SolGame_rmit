using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Behavior/Composite")]
public class Composite : BoidsBehavior
{
    public BoidsBehavior[] behaviors;
    public float[] weights;


    public override Vector3 CalMove(BoidManager manager, List<Transform> context, BoidSpawnerTest BoidSpawner)
    {
        if (weights.Length != behaviors.Length)
        {
            Debug.Log("Data mismatch in " + name, this);
            return Vector3.zero;
        }

        Vector3 move = Vector3.zero;
        //iterate through behaviors
        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector3 partialMove = behaviors[i].CalMove(manager, context, BoidSpawner) * weights[i];
            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }
        return move;
    }
}
