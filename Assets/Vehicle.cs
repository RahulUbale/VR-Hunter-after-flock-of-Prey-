using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public Vector3 steering;

    public float mass = 1;
    public float maxSpeed = 5;
    public float minSpeed = 5;
    public float banking = 5;

    public float maxSteering = .2f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveVehicle()
    {
        steering = Vector3.ClampMagnitude(steering, maxSteering);

        velocity += steering;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        this.transform.position += velocity * Time.deltaTime;

        Vector3 newUp = Vector3.Lerp(transform.up, (Vector3.up * 9.8f) + (steering * banking), 0.05f);

        //this.transform.forward = velocity;
        this.transform.rotation = Quaternion.LookRotation(velocity, newUp);
        
    }
}
