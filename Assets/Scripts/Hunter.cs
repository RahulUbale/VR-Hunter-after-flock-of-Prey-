using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public List<Vehicle> boids;
    Vehicle vehicle;




    // Start is called before the first frame update
    void Start()
    {
        vehicle = GetComponent<Vehicle>();
        
    }

    // Update is called once per frame
    void Update()
    {
        vehicle.steering = Vector3.zero;
        vehicle.steering += SteeringBehaviors.CalculateCohesion(vehicle,boids);
        vehicle.MoveVehicle();


        
    }
}
