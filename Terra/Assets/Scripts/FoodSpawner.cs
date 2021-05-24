using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [System.Serializable]
    public class FoodItem {
        public GameObject food;
        public int probability;
    }

    [SerializeField]
    private int foodCount;
    [SerializeField]
    private FoodItem[] foodList;

    private GameObject[] foodListWithProbability;

    private Map mapInstance;
    public static FoodSpawner instance;

    void Awake(){
        if(instance != null){
            Debug.LogError("More than one food spawner in scene!");
            return;
        }
        instance = this;
    }

    void Start(){
        this.mapInstance = Map.instance;
        this.prepareFoodListWithProbability();
        for(int i = 0; i < foodCount; i++){
            SpawnRandomFood();
        }
    }

    private void prepareFoodListWithProbability(){
        int count;
        int probabilityCount = 0;
        int foodListProbabilityIndex = 0;
        GameObject food;
        for(int k = 0; k < foodList.Length; k++){
            probabilityCount += foodList[k].probability;
        }
        this.foodListWithProbability = new GameObject[probabilityCount];
        for(int i = 0; i < foodList.Length; i++){
            count = foodList[i].probability;
            food = foodList[i].food;
            for(int j = 0; j < count; j++){
                foodListWithProbability[foodListProbabilityIndex] = food;
                foodListProbabilityIndex++;
            }
        }
    }

    public void SpawnRandomFood(){
        int foodIndex = Random.Range(0, foodListWithProbability.Length);
        GameObject food = (GameObject) Instantiate(foodListWithProbability[foodIndex], transform);
        float randomXPosition = Random.Range(this.mapInstance.gridPosition["left"], this.mapInstance.gridPosition["right"]);
        float randomYPosition = Random.Range(this.mapInstance.gridPosition["bottom"], this.mapInstance.gridPosition["top"]);
        food.transform.position = new Vector3(randomXPosition, randomYPosition, food.transform.position.z);
    }

    public void setCount(int count){
        this.foodCount = count;
    }
    

}
