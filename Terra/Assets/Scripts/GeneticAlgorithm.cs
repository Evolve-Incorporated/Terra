using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    [SerializeField]
    public Generation generation;
    public static GeneticAlgorithm instance;
    // Start is called before the first frame update

    public void Run() {
        //Debug.Log("Creating creatures");
        //List<GameObject> spawnedCreatures = CreatureSpawner.instance.SpawnCreatures().ToList(); //błąd tu jest jakiś
        //List<Creature> spawnedCreatures = new List<Creature>(GameObject.FindObjectsOfType<Creature>());
        //Debug.Log("Created " + spawnedCreatures.Count + " creatures");
        //generation = new Generation(spawnedCreatures.Select(creature => creature.GetComponent<Creature>().GetDNA()).ToList());
        generation = new Generation();
        generation.Run();
    }

    void Awake(){
        if(instance != null){
            Debug.LogError("More than one genetic algorithm in scene!");
            return;
        }
        instance = this;
    }

    // void Start() {
    //     Run();
    // }

    // Update is called once per frame
    void Update()
    {
        //check if generation is done
        if (generation.totalAlive == 0) {
            generation.End();
            generation = generation.Next();
            generation.Run();
        } 
        
    }
}
