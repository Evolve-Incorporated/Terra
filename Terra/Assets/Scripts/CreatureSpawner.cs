using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{

    [SerializeField]
    public int creaturesCount;

    [SerializeField]
    private GameObject[] creatures;


    private int idCounter;
    private Map mapInstance;
    public static CreatureSpawner instance;


    void Awake(){
        if(instance != null){
            Debug.LogError("More than one creature spawner in scene!");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        idCounter = 0;
        this.mapInstance = Map.instance;
        GeneticAlgorithm.instance.Run();
    }

    public List<GameObject> SpawnCreatures(int customCount = -1){
        int count = customCount != -1 ? customCount : creaturesCount;
        List<GameObject> spawnedObjects = new List<GameObject>();
        Debug.Log("About to spawn " + creaturesCount + " creatures");
        for (int i = 0; i < count; i++) spawnedObjects.Add(SpawnCreature());

        return spawnedObjects;
    }

    public GameObject SpawnCreature(){
        Debug.Log("Spawning random creature");
        int creatureIndex = Random.Range(0, creatures.Length);

        GameObject creature = (GameObject) Instantiate(creatures[creatureIndex], transform);

        Creature creatureComponent = creature.GetComponent<Creature>(); 
        creatureComponent.SetDNA(DNA.RandomDNA()); // tutaj dna kriczera jest ustawiane na randomowe
        creatureComponent.RefillEnergy(); // tutaj energia kriczera jest ustawiana na wartosc rowna genowi maxEnergy

        idCounter += 1;
        creatureComponent.id = idCounter;

        float randomXPosition = Random.Range(this.mapInstance.gridPosition["left"], this.mapInstance.gridPosition["right"]);
        float randomYPosition = Random.Range(this.mapInstance.gridPosition["bottom"], this.mapInstance.gridPosition["top"]);
        creature.transform.position = new Vector3(randomXPosition, randomYPosition, creature.transform.position.z);
        

        return creature;
    }

    public GameObject SpawnCreature(DNA dna, bool useParentDNA = false){
        int creatureIndex = Random.Range(0, creatures.Length);

        GameObject creature = (GameObject) Instantiate(creatures[creatureIndex], transform);

        Creature creatureComponent = creature.GetComponent<Creature>();
        DNA _dna = useParentDNA == false ? new DNA(dna) : dna;
        creatureComponent.SetDNA(_dna); // tutaj dna kriczera jest ustawiane na kopie dna podanego w argumencie
        creatureComponent.RefillEnergy(); // tutaj energia kriczera jest ustawiana na wartosc rowna genowi maxEnergy

        idCounter += 1;
        creatureComponent.id = idCounter;

        float randomXPosition = Random.Range(this.mapInstance.gridPosition["left"], this.mapInstance.gridPosition["right"]);
        float randomYPosition = Random.Range(this.mapInstance.gridPosition["bottom"], this.mapInstance.gridPosition["top"]);
        creature.transform.position = new Vector3(randomXPosition, randomYPosition, creature.transform.position.z);
        

        return creature;
    }

    public GameObject SpawnCreature(Vector3 position, DNA dna){
        int creatureIndex = Random.Range(0, creatures.Length);
        GameObject creature = (GameObject) Instantiate(creatures[creatureIndex], transform);
        creature.transform.position = new Vector3(position.x, position.y, position.z);

        Creature creatureComponent = creature.GetComponent<Creature>();
        creatureComponent.SetDNA(new DNA(dna)); // tutaj dna kriczera jest ustawiane na kopie dna podanego w argumencie
        creatureComponent.RefillEnergy(); // tutaj energia kriczera jest ustawiana na wartosc rowna genowi maxEnergy

        idCounter += 1;
        creatureComponent.id = idCounter;

        return creature;
    }
}
