using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [System.Serializable]
    public class FoodItem {
        public GameObject food;
        public float probability;
    }

    [SerializeField]
    private int foodCount;
    [SerializeField]
    private FoodItem[] foodList;

    private Map mapInstance;

    void Start(){
        this.mapInstance = Map.instance;
        Debug.Log(this.mapInstance);
        for(int i = 0; i < foodCount; i++){
            SpawnRandomFood();
        }
    }

    public void SpawnRandomFood(){
        int foodIndex = Random.Range(0, foodList.Length);
        Debug.Log(foodList[foodIndex].food);
        GameObject food = (GameObject) Instantiate(foodList[foodIndex].food, transform);
        float randomXPosition = Random.Range(this.mapInstance.gridPosition["left"], this.mapInstance.gridPosition["right"]);
        float randomYPosition = Random.Range(this.mapInstance.gridPosition["bottom"], this.mapInstance.gridPosition["top"]);
        food.transform.position = new Vector3(randomXPosition, randomYPosition, food.transform.position.z);
    }

}
