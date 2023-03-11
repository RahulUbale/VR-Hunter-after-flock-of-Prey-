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

        float relativeHeading = Vector3.Angle(vehicle.velocity, target.velocity);

        float angleToTarget = Vector3.Angle(vehicle.velocity, direction);

        if (relativeHeading < 18 && angleToTarget > 90)
        {
            return CalculateSeek(vehicle, target.transform.position);
        }

        return CalculateSeek(vehicle, target.transform.position + (target.velocity * lookAhead));
    }

    private static void VisualizeForces(Vehicle vehicle, Vector3 desiredVelocity)
    {
        Debug.DrawRay(vehicle.transform.position, vehicle.velocity, Color.green);
        Debug.DrawRay(vehicle.transform.position, desiredVelocity, Color.red);
        Debug.DrawRay(vehicle.transform.position, vehicle.steering, Color.blue);
    }
}
