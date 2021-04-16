using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    [SerializeField]
    private float energy;
    [SerializeField]
    private float probability;

    private FoodSpawner foodSpawnerInstance;
    void Start()
    {
        foodSpawnerInstance = FoodSpawner.instance;
    }

    public void Consume(){
        foodSpawnerInstance.SpawnRandomFood();
        Destroy(gameObject);
    }

    public float GetEnergy(){
        return energy;
    }
}
