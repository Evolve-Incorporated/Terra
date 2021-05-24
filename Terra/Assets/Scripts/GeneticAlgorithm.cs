using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithm : MonoBehaviour
{
    [SerializeField]
    public float mutationRate = 0.1f;
    [SerializeField]
    public float mutationStrength = 0.1f;
    [SerializeField]
    public float percentToPassSelection = 0.5f;
    [SerializeField]
    public int minSelectionCount = 3;
    [SerializeField]
    public float maxGenerationDurationSeconds = 20;
    [SerializeField]
    public float globalMoveCostMultiplier = 0.15f;

    public Text highscoreText;

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
        bool timeIsUp = generation.TimeElapsed() >= maxGenerationDurationSeconds;
        if (!generation.IsAlive() || timeIsUp) {
            if (timeIsUp) Debug.Log("Generation reached it's max lifetime.");
            else Debug.Log("Generation died.");

            generation.End();
            SetHighscore();
            generation = generation.Next();
            generation.Run();
        } 
        
    }

    public void SetHighscore(){
        List<DNA> DNAList = generation.GetSortedDNAList();
        highscoreText.text = DNAList[0].score.ToString();
    }
}
