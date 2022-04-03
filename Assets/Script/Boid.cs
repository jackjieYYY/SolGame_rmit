using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    static public List<Boid> boids;

    public Vector3 velocity;        
    public Vector3 newVelocity;     
    public Vector3 newPosition;

    float randomStrength = 2.0f;

    float crashRadius = 2.0f;
    float swarmRadius = 32.0f;

    public Boid closest;                

    private void Awake()
    {
        
        if (boids == null)
            boids = new List<Boid>();

        
        boids.Add(this);

        // Place our boid in a random position
        Vector3 randPos = Random.insideUnitSphere * BoidSpawner.S.spawnRadius;
        randPos.y = 1;
        this.transform.position = randPos;

        
        // Give them a random initial velocity
        velocity = Random.onUnitSphere;
        velocity *= BoidSpawner.S.spawnVelcoty;

        
        Color randColor = Color.black;

        while (randColor.r + randColor.g + randColor.b < 1.0f)
            randColor = new Color(Random.value, Random.value, Random.value);
        
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            r.material.color = randColor;
    }

    private void Update()
    {
        List<Boid> neighbours = Getneighbours(this);
        List<Boid> crashRisks = GetCrashRisks(this);

        newVelocity = velocity;
        newPosition = this.transform.position;


        Vector3 randVelocity = Random.onUnitSphere;
        newVelocity += randVelocity * BoidSpawner.S.randomAmt;

        // Try move with a uniform pattern
        Vector3 neighbourVel = GetAverageVelocity(neighbours);
        newVelocity += neighbourVel * BoidSpawner.S.velocityMatchingAmt;

        // Try move towards the same goal
        Vector3 neighbourCenterOffset = GetAveragePosition(neighbours) - this.transform.position;
        newVelocity += neighbourCenterOffset * BoidSpawner.S.flockCenteringAmt;

        // Avoid each other
        Vector3 dist;
        if (crashRisks.Count > 0) 
        {
            Vector3 averageCrashVec = GetAveragePosition(crashRisks);
            dist = averageCrashVec - this.transform.position;
            newVelocity -= dist * BoidSpawner.S.crashAvoidanceAmt;
        }

        // Aim at the player, if they're alive!!!!
        if (BoidSpawner.S.master)
        {
            dist = BoidSpawner.S.master.transform.position - this.transform.position;
            newVelocity += dist.normalized;
        }

    }

    private void LateUpdate()
    {
        // Update our boid's position based on the velocity we calculated
        velocity = (1 - BoidSpawner.S.velocityLerpAmt) * velocity + BoidSpawner.S.velocityLerpAmt * newVelocity;

        // Check if we're above max velocity
        if (velocity.magnitude > BoidSpawner.S.maxVelocity)
            velocity = velocity.normalized * BoidSpawner.S.maxVelocity;
        if (velocity.magnitude < BoidSpawner.S.minVelocity)
            velocity = velocity.normalized * BoidSpawner.S.minVelocity;

        newPosition = this.transform.position + velocity * Time.deltaTime;
        newPosition.y = 0.0f;

        this.transform.LookAt(newPosition);

        this.transform.position = newPosition;;

    }

    public List<Boid> Getneighbours(Boid boi)
    {
        List<Boid> neighbours = new List<Boid>();
        
        Vector3 delta;              
        float dist;

        neighbours.Clear();    

        foreach (Boid b in boids)
        {
            if (b == boi) // Skip if we're matching ourselves
                continue;

            delta = b.transform.position - boi.transform.position; 
            dist = delta.magnitude;

            if (dist < swarmRadius) // Are they in our radius
                neighbours.Add(b);
        }
        //Debug.Log("Neighbours: "+neighbours.Count);
        return (neighbours);
    }
    public List<Boid> GetCrashRisks(Boid boi)
    {
        List<Boid> crashRisks = new List<Boid>();
        Vector3 delta;
        float dist;

        crashRisks.Clear();

        foreach (Boid b in boids)
        {
            if (b == boi) // Skip if we're matching ourselves
                continue;

            delta = b.transform.position - boi.transform.position;
            dist = delta.magnitude;

            if (dist < crashRadius) // Are they in our radius
                crashRisks.Add(b);
        }
        //Debug.Log("Crash Risks: " + crashRisks.Count);

        return (crashRisks);
    }


    public Vector3 GetAveragePosition(List<Boid> someBoids)
    {
        Vector3 sum = Vector3.zero;
        foreach (Boid b in someBoids)
            sum += b.transform.position;
        Vector3 center = sum / someBoids.Count;

        return (center);
    }

   
    public Vector3 GetAverageVelocity(List<Boid> someBoids)
    {
        Vector3 sum = Vector3.zero;
        foreach (Boid b in someBoids)
            sum += b.velocity;
        Vector3 avg = sum / someBoids.Count;

        return (avg);
    }
}
