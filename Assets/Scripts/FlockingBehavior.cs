using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingBehavior : MonoBehaviour
{
    public List<Vehicle> boids;
    public Transform hunter;

    [Range(0, 5)]
    public float separationForce = 1;
    [Range(0, 5)]
    public float cohesionForce = 1;
    [Range(0, 5)]
    public float aligmentForce = 1;   
    [Range(1, 30)]
    public float separationDistance = 5;
    [Range(-5, 10)]
    public float homeForce = 1;
    public float flockRadius = 10;
    public float neighborhoodRadius = 10;

    public float maxSpeed = 40;
    public float minSpeed = 20;
    public float maxSteering = 5;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Vehicle boid in boids)
        {
            boid.velocity = Random.onUnitSphere * boid.maxSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Vehicle boid in boids)
        {
            AdjustBoidParameters(boid);
            List<Vehicle> neighbors = FindNeighbors(boid, neighborhoodRadius);

            boid.steering = Vector3.zero;
            boid.steering += SteeringBehaviors.CalculateSeparation(boid, neighbors, separationDistance) * separationForce;
            boid.steering += SteeringBehaviors.CalculateCohesion(boid, neighbors) * cohesionForce * Time.deltaTime;
            boid.steering += SteeringBehaviors.CalculateAlignment(boid, neighbors) * aligmentForce * Time.deltaTime;
            float dist = (boid.transform.position - this.transform.position).sqrMagnitude;

            if (dist > flockRadius * flockRadius)
            {
                boid.steering += SteeringBehaviors.CalculateSeek(boid, this.transform.position) * Time.deltaTime * homeForce;
            }
            



            float distFromHunter = Vector3.Distance(boid.transform.position, hunter.position);
            if(distFromHunter < 10)
            {
                boid.steering += SteeringBehaviors.CalculateFlee(boid, hunter.transform.position);
            }






            boid.MoveVehicle();
        }
    }

    void AdjustBoidParameters(Vehicle boid)
    {
        boid.maxSpeed = maxSpeed;
        boid.minSpeed = minSpeed;
        boid.maxSteering = maxSteering;
    }

    List<Vehicle> FindNeighbors(Vehicle vehicle, float radius)
    {
        List<Vehicle> neighbors = new List<Vehicle>();
        Collider[] neighborColliders = Physics.OverlapSphere(vehicle.transform.position, radius);

        foreach (Collider neighbor in neighborColliders)
        {
            Vehicle boid = neighbor.GetComponent<Vehicle>();
            if (boid != null)
            {
                neighbors.Add(boid);
            }
        }
        return neighbors;
    }
}
