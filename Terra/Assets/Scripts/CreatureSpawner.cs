using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{

    [SerializeField]
    private int creaturesCount;
    [SerializeField]
    private GameObject[] creatures;

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
        this.mapInstance = Map.instance;
        for(int i = 0; i < creaturesCount; i++){
            SpawnCreature();
        }
    }

    public GameObject SpawnCreature(){
        int creatureIndex = Random.Range(0, creatures.Length);
        GameObject creature = (GameObject) Instantiate(creatures[creatureIndex], transform);
        float randomXPosition = Random.Range(this.mapInstance.gridPosition["left"], this.mapInstance.gridPosition["right"]);
        float randomYPosition = Random.Range(this.mapInstance.gridPosition["bottom"], this.mapInstance.gridPosition["top"]);
        creature.transform.position = new Vector3(randomXPosition, randomYPosition, creature.transform.position.z);
        
        Creature creatureComponent = creature.GetComponent<Creature>(); 
        creatureComponent.SetDNA(new DNA()); // tutaj dna kriczera jest ustawiane na puste
        DNA.RandomizeCreatureDNA(creatureComponent); // tutaj geny są randomowane
        creatureComponent.RefillEnergy(); // tutaj energia kriczera jest ustawiana na wartosc rowna genowi maxEnergy

        return creature;
    }

    public GameObject SpawnCreature(Vector3 position, DNA dna){
        int creatureIndex = Random.Range(0, creatures.Length);
        GameObject creature = (GameObject) Instantiate(creatures[creatureIndex], transform);
        creature.transform.position = new Vector3(position.x, position.y, position.z);

        Creature creatureComponent = creature.GetComponent<Creature>();
        creatureComponent.SetDNA(new DNA(dna)); // tutaj dna kriczera jest ustawiane na kopie dna podanego w argumencie
        creatureComponent.RefillEnergy(); // tutaj energia kriczera jest ustawiana na wartosc rowna genowi maxEnergy

        return creature;
    }
}
