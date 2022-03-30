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

    public List<Boid> neighbors;        
    public List<Boid> collisionRisks;   
    public Boid closest;                

    private void Awake()
    {
        
        if (boids == null)
            boids = new List<Boid>();

        
        boids.Add(this);

        
        Vector3 randPos = Random.insideUnitSphere * BoidSpawner.S.spawnRadius;

        
        randPos.y = 1;
        this.transform.position = randPos;

        
        velocity = Random.onUnitSphere * randomStrength;
        velocity *= BoidSpawner.S.spawnVelcoty;

        
        neighbors = new List<Boid>();
        collisionRisks = new List<Boid>();

        
        Color randColor = Color.black;

        while (randColor.r + randColor.g + randColor.b < 1.0f)
            randColor = new Color(Random.value, Random.value, Random.value);
        
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            r.material.color = randColor;
    }

    private void Update()
    {
        List<Boid> neighbors = GetNeighbors(this);

        newVelocity = velocity;
        newPosition = this.transform.position;


        Vector3 neighborVel = GetAverageVelocity(neighbors);
       
        newVelocity += neighborVel * BoidSpawner.S.velocityMatchingAmt;

        Vector3 neighborCenterOffset = GetAveragePosition(neighbors) - this.transform.position;

        newVelocity += neighborCenterOffset * BoidSpawner.S.flockCenteringAmt;

        Vector3 dist;
        if (collisionRisks.Count > 0) 
        {
            Vector3 collisionAveragePos = GetAveragePosition(collisionRisks);
            dist = collisionAveragePos - this.transform.position;
            newVelocity += dist * BoidSpawner.S.collisionAvoidanceAmt;
        }

        dist = BoidSpawner.S.master.transform.position - this.transform.position;


        newVelocity += dist.normalized;

    }

    private void LateUpdate()
    {
        velocity = (1 - BoidSpawner.S.velocityLerpAmt) * velocity + BoidSpawner.S.velocityLerpAmt * newVelocity;

        // Check if we're above max velocity
        if (velocity.magnitude > BoidSpawner.S.maxVelocity)
            velocity = velocity.normalized * BoidSpawner.S.maxVelocity;
        if (velocity.magnitude < BoidSpawner.S.minVelocity)
            velocity = velocity.normalized * BoidSpawner.S.minVelocity;

        newPosition = this.transform.position + velocity * Time.deltaTime;


        this.transform.LookAt(newPosition);

        this.transform.position = newPosition;
    }

    public List<Boid> GetNeighbors(Boid boi)
    {
        Vector3 delta;              
        float dist;

        float swarmRadius = 32.0f;

        neighbors.Clear();    

        foreach (Boid b in boids)
        {
            if (b == boi) // Skip if we're matching ourselves
                continue;

            delta = b.transform.position - boi.transform.position; 
            dist = delta.magnitude;

            if (dist < swarmRadius) // Are they in our radius
                neighbors.Add(b);
        }


        return (neighbors);
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
