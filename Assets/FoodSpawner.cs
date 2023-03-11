using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    List<GameObject> foodList = new List<GameObject>();
    public int numPellets = 20;

    // Start is called before the first frame update
    void Start()
    {
        GenerateFood();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GetFood()
    {
        return foodList;
    }

    public void RemoveFood(GameObject food)
    {
        foodList.Remove(food);
        Destroy(food);
    }

    void GenerateFood()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        Vector3 minBounds = GetComponent<MeshRenderer>().bounds.min;
        Vector3 maxBounds = GetComponent<MeshRenderer>().bounds.max;
        for (int i = 0; i < numPellets; i++)
        {
            GameObject food = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float xRand = Random.Range(minBounds.x, maxBounds.x);
            float zRand = Random.Range(minBounds.z, maxBounds.z);
            food.transform.position = new Vector3(xRand, 0, zRand);

            foodList.Add(food);
        }
    }
}
