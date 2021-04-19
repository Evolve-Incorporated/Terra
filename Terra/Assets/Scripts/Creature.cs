using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{

    [SerializeField]
    private float energy;

    [SerializeField]
    private DNA dna;


    enum State {Waiting, Moving}
    private State actualState;
    private Vector3 nextPosition;
    private Map mapInstance;
    private CreatureSpawner creatureSpawnerInstance;
    private float mapOffset = 0.5f;
    private string foodTag = "Food";

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        actualState = State.Waiting;
        this.mapInstance = Map.instance;
        creatureSpawnerInstance = CreatureSpawner.instance;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(actualState == State.Waiting ){
            NextPosition();
            TurnAround();
            actualState = State.Moving;
        }else if(actualState == State.Moving){
            Move();
        }
        
        energy = energy - Time.deltaTime * dna.GetReproductionCost(); 
        if(energy <= 0){
            die();
        }

        // przenioslem reprodukcje
        

    }

    void TurnAround(){
        if(nextPosition.x > transform.position.x){
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }else{
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
    }

    void NextPosition(){
        float xPositionRandom = Random.Range(-10f, 10f);
        float yPositionRandom = Random.Range(-10f, 10f);
        Vector2 nextPossiblePosition = new Vector2(transform.position.x + xPositionRandom, transform.position.y + yPositionRandom);
        if(nextPossiblePosition.x > this.mapInstance.gridPosition["right"] - mapOffset){
            nextPossiblePosition.x = this.mapInstance.gridPosition["right"] - mapOffset;
        }
        if(nextPossiblePosition.x < this.mapInstance.gridPosition["left"] + mapOffset){
            nextPossiblePosition.x = this.mapInstance.gridPosition["left"] + mapOffset;
        }
        if(nextPossiblePosition.y > this.mapInstance.gridPosition["top"] - mapOffset){
            nextPossiblePosition.y = this.mapInstance.gridPosition["top"] - mapOffset;
        }
        if(nextPossiblePosition.y < this.mapInstance.gridPosition["bottom"] + mapOffset){
            nextPossiblePosition.y = this.mapInstance.gridPosition["bottom"] + mapOffset;
        }
        this.nextPosition = new Vector3(nextPossiblePosition.x, nextPossiblePosition.y, transform.position.z);
    }

    void Move(){
        Vector3 direction = nextPosition - transform.position;
        transform.Translate(direction.normalized * dna.getGene("speed") * Time.deltaTime, Space.World);

        if(Vector2.Distance(transform.position, nextPosition) <= 0.2f)
        {
            actualState = State.Waiting;
            if(target && Vector2.Distance(transform.position, target.position) <= 0.2f){
                energy = Mathf.Clamp(energy + target.gameObject.GetComponent<Food>().GetEnergy(), 0 , dna.getGene("maxEnergy")); //// clamp  wartosci energii miedzy 0 - maxEnergy
                target.gameObject.GetComponent<Food>().Consume();
                target = null;
                
                if(energy >= dna.GetReproductionCost()){
                    //Debug.Log("Energy: " + energy.ToString() + " Cost: " + dna.GetReproductionCost().ToString());
                    Reproduce();
                }
            }
        }
    }

    void UpdateTarget(){
        GameObject[] foodList = GameObject.FindGameObjectsWithTag(foodTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestFood = null;
        foreach(GameObject food in foodList){
            float distanceToFood = Vector3.Distance(transform.position, food.transform.position);
            if(distanceToFood < shortestDistance){
                shortestDistance = distanceToFood;
                nearestFood = food;
            }
        }

        if(nearestFood != null && shortestDistance <= dna.getGene("range")){
            target = nearestFood.transform;
            this.nextPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            TurnAround();
            actualState = State.Moving;
        }else{
            target = null;
        }
    }
    
    void Reproduce() {
        GameObject creature = creatureSpawnerInstance.SpawnCreature(transform.position, dna);
        Creature creatureComponent = creature.GetComponent<Creature>();
        
        creatureComponent.Mutate();
        creatureComponent.RefillEnergy();
        Debug.Log("Reproducing:\n" + dna + "\nEnergy: " + energy + "\n\nInto:\n" + creatureComponent.GetDNA() + "\nEnergy: " + creatureComponent.energy);
        
        energy -= dna.GetReproductionCost();
    }

    void Mutate() {
        DNA.MutateCreatureDNA(this);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dna.getGene("range"));
    }

    private void die(){
        Destroy(gameObject);
    }


    /// zwiazane z DNA

    public void RefillEnergy() {
        energy = GetDNA().getGene("maxEnergy");
    }


    /// wrappery do DNA
    public DNA GetDNA() {
        return dna;
    }


    public void SetDNA(DNA newDNA) {
        dna = newDNA;
    }

}
