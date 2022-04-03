using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This object will create all of our boids and allow for easily accessible / tweakable settings


public class BoidSpawner : MonoBehaviour
{
    static public BoidSpawner S;

    public int numBoids = 100;
    public GameObject boidPrefab;
    public GameObject master; // Will get followed
    public float spawnRadius = 50;
    public float spawnVelcoty = 1f;
    public float minVelocity = 0f;
    public float maxVelocity = 2f;
    public float crashRadius = 2.0f;
    public float swarmRadius = 32.0f;
    public float randomAmt = 0.1f;
    public float velocityMatchingAmt = 0.15f;
    public float flockCenteringAmt = 0.12f;
    public float crashAvoidanceAmt = 1.5f;
    public float velocityLerpAmt = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        S = this;

        for (int i = 0; i < numBoids; i++)
            Instantiate(boidPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
