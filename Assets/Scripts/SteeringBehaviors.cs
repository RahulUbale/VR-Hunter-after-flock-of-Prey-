using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SteeringBehaviors
{
    public static Vector3 CalculateSeek(Vehicle vehicle, Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - vehicle.transform.position;
        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= vehicle.maxSpeed;
        //VisualizeForces(vehicle, desiredVelocity);

        return desiredVelocity - vehicle.velocity;
    }

    public static Vector3 CalculateFlee(Vehicle vehicle, Vector3 targetPos)
    {
        Vector3 desiredVelocity = vehicle.transform.position - targetPos;
        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= vehicle.maxSpeed;

        VisualizeForces(vehicle, desiredVelocity);

        return desiredVelocity - vehicle.velocity;
    }

    /// <summary>
    /// Force that steers a vehicle toward a target, slowing as it gets closer to the target
    /// </summary>
    /// <param name="vehicle">The vehicle to be moved</param>
    /// <param name="targetPos"></param>
    /// <param name="slowingDistance"></param>
    /// <returns></returns>
    public static Vector3 CalculateArrive(Vehicle vehicle, Vector3 targetPos, float slowingDistance = 10)
    {
        Vector3 direction = targetPos - vehicle.transform.position;
        float distance = direction.magnitude;

        if (distance > slowingDistance)
        {
            return CalculateSeek(vehicle, targetPos);
        }

        float rampedSpeed = vehicle.maxSpeed * (distance / slowingDistance);
        Vector3 desiredVelocity = (direction / distance) * rampedSpeed;

        return desiredVelocity - vehicle.velocity;
    }

    public static Vector3 CalculatePursue(Vehicle vehicle, Vehicle target)
    {
        Vector3 direction = target.transform.position - vehicle.transform.position;
        float distance = direction.magnitude;

        float lookAhead = distance / (vehicle.velocity.magnitude + target.velocity.magnitude);

        float relativeHeading = Vector3.Angle(vehicle.velocity, target.velocity); // are we headed in the same direction?

        float angleToTarget = Vector3.Angle(vehicle.velocity, direction); // if this is greater than 90, we're ahead of the target

        // If we're ahead of the target and moving in the same general direction, we want to seek the target itself
        if (relativeHeading < 18 && angleToTarget > 90)
        {
            return CalculateSeek(vehicle, target.transform.position);
        }

        return CalculateSeek(vehicle, target.transform.position + (target.velocity * lookAhead));
    }

    /// <summary>
    /// Force to keep boids from bumping into neighbors
    /// </summary>
    /// <param name="vehicle">The vehicle we're applying force to</param>
    /// <param name="neighbors">List of local neighbors</param>
    /// <param name="separationDistance">Raius within which force will be applied</param>
    /// <returns>Separation Force</returns>
    public static Vector3 CalculateSeparation(Vehicle vehicle, List<Vehicle> neighbors, float separationDistance = 3)
    {
        Vector3 steering = Vector3.zero;

        foreach (Vehicle neighbor in neighbors)
        {
            if (neighbor != vehicle)
            {
                // Calculate the direction and distance from our neighbor
                Vector3 toAgent = vehicle.transform.position - neighbor.transform.position;
                float dist = toAgent.magnitude;

                // Only apply the separation force if boids are within the separation radius
                if (dist < separationDistance)
                {
                    steering += toAgent.normalized / dist; // the closer we are, the more force we need to apply
                }               
            }
        }

        return steering;
    }

    /// <summary>
    /// Seek the center of mass of our closest neighbors
    /// </summary>
    /// <param name="vehicle">The vehicle we're applying force to</param>
    /// <param name="neighbors">List of local neighbors</param>
    /// <returns>Steering Force toward center of mass</returns>
    public static Vector3 CalculateCohesion(Vehicle vehicle, List<Vehicle> neighbors)
    {
        Vector3 centerOfMass = Vector3.zero;
        int neighborCount = 0;

        // We want to find the average position of our neighbors,
        // so we loop through our neighbors, add their positions together, and divide by the number of neighbors
        foreach (Vehicle neighbor in neighbors)
        {
            if (neighbor != vehicle) // Don't count ourselves
            {
                centerOfMass += neighbor.transform.position;
                neighborCount += 1;
            }
        }

        if (neighborCount > 0) // Don't divide by 0!
        {
            centerOfMass /= neighborCount;
        }

        // We want to seek the center of mass
        return CalculateSeek(vehicle, centerOfMass);
    }

    /// <summary>
    /// Force that changes current velocity toward the velocity of local neighbors
    /// </summary>
    /// <param name="vehicle">The vehicle we're moving</param>
    /// <param name="neighbors">List of neighbors</param>
    /// <returns>Steering Force</returns>
    public static Vector3 CalculateAlignment(Vehicle vehicle, List<Vehicle> neighbors)
    {
        // We want to steer in the same direction as our neighbors
        Vector3 avgVelocity = Vector3.zero;
        int neighborCount = 0;

        // Loop over neighbors to add up total velocity vector
        foreach (Vehicle neighbor in neighbors)
        {
            if (neighbor != vehicle) // Don't count our own velocity
            {
                avgVelocity += neighbor.velocity;
                neighborCount += 1;
            }
        }

        if (neighborCount > 0) // Don't divide by 0!
        {
            avgVelocity /= neighborCount; // Find the average velocity
        }

        // We want to return the vector that turns our current velocity to the average velocity
        avgVelocity -= vehicle.velocity;
        return avgVelocity;
    }

    // We can call this if we want to see our forces
    private static void VisualizeForces(Vehicle vehicle, Vector3 desiredVelocity)
    {
        Debug.DrawRay(vehicle.transform.position, vehicle.velocity, Color.green);
        Debug.DrawRay(vehicle.transform.position, desiredVelocity, Color.red);
        Debug.DrawRay(vehicle.transform.position, vehicle.steering, Color.blue);
    }
}
