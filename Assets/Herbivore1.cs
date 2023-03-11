using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore1 : MonoBehaviour
{



    float hunger = 50;
    enum State { Forage, Home };
    Vehicle vehicle;
    public Transform Home;

    State state = State.Forage;



    // Start is called before the first frame update
    void Start()
    {
        vehicle = GetComponent<Vehicle>();
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= Time.deltaTime;

        if (state == State.Forage)
        {


            GameObject closestFood = FindClosestFood();
            if (closestFood != null)
            {
                vehicle.steering = SteeringBehaviors.CalculateSeek(vehicle, closestFood.transform.position);
                vehicle.MoveVehicle();
                if (hunger > 50)
                {
                    state = State.Home;

                }
            }
            else state = State.Home;


        }
        else if (state == State.Home)
        {
            vehicle.steering = SteeringBehaviors.CalculateArrive(vehicle, Home.position);
            vehicle.MoveVehicle();
            if (hunger < 50)
            {
                state = State.Forage;
            }
        }




    }




    GameObject FindClosestFood()
    {
        GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");
        float closestDistance = float.MaxValue;
        GameObject closestFood = null;


        foreach (GameObject food in allFood)
        {
            float distance = (transform.position - food.transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFood = food;
            }

        }
        return closestFood;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
            hunger += 10;

        }
    }
}
