using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField]
    public float energy;
    public int creatureIndex;

    [SerializeField]
    public float score = 0;

    [SerializeField]
    public int id;

    [SerializeField]
    public DNA dna = new DNA();


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
        InvokeRepeating("UpdateTarget", 0f, 0.01f);
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

        BurnEnergy(Time.deltaTime * dna.GetMoveCost());

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

    void Eat(GameObject target) {
        float foodEnergy = target.gameObject.GetComponent<Food>().GetEnergy();
        CommitEnergy(foodEnergy);
        target.gameObject.GetComponent<Food>().Consume();
        target = null;
        // if(energy >= dna.GetReproductionCost()){
        //     Reproduce();
        // }
    }

    public void CommitEnergy(float energy) {
        this.energy = Mathf.Clamp(this.energy + energy, 0 , dna.getGene("maxEnergy")); //// clamp  wartosci energii miedzy 0 - maxEnergy
        dna.score += 1;
    }

    void Move(){
        Vector3 direction = nextPosition - transform.position;
        transform.Translate(direction.normalized * dna.getGene("speed") * Time.deltaTime, Space.World);

        if(Vector2.Distance(transform.position, nextPosition) <= 0.2f)
        {
            actualState = State.Waiting;
            if(target && Vector2.Distance(transform.position, target.position) <= 0.2f){
                Eat(target.gameObject);
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
        BurnEnergy(dna.GetReproductionCostAfterDiscount());
        GameObject creature = creatureSpawnerInstance.SpawnCreature(transform.position, dna);
        Creature creatureComponent = creature.GetComponent<Creature>();
        if (UnityEngine.Random.Range(0f, 1f) > GeneticAlgorithm.instance.mutationRate) creatureComponent.Mutate();
        GeneticAlgorithm.instance.generation.totalAlive += 1;
    }

    void Mutate() {
        DNA.MutateCreatureDNA(this);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dna.getGene("range"));
    }

    public void BurnEnergy(float amount, string message = null) {
        if (message != null) Debug.Log(id + ". " + "[" + message + "]" + "Zabrano " + energy + " energii.");
        energy -= amount;
    }

    public void die(){
        Destroy(gameObject);
        GeneticAlgorithm.instance.generation.totalAlive -= 1;
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
