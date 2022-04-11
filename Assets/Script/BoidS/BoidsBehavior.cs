using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoidsBehavior : ScriptableObject
{
    public abstract Vector3 CalMove(BoidManager manager, List<Transform> context, BoidSpawnerTest BoidSpawner);

}
