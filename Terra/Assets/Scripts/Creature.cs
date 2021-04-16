using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float range;
    [SerializeField]
    private float energy;
    [SerializeField]
    private float reproduceCost;
    [SerializeField]
    private float tendencyToReproduce;


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
        
        energy = energy - Time.deltaTime * 5;
        if(energy <= 0){
            die();
        }

        if(energy >= reproduceCost * tendencyToReproduce){
            Reproduce();
        }

    }

    void TurnAround(){
        if(nextPosition.x > transform.position.x){
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }else{
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
    }

    void NextPosition(){
        float xPositionRandom = Random.Range(-5f, 5f);
        float yPositionRandom = Random.Range(-5f, 5f);
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
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if(Vector2.Distance(transform.position, nextPosition) <= 0.2f)
        {
            actualState = State.Waiting;
            if(target && Vector2.Distance(transform.position, target.position) <= 0.2f){
                energy = energy + target.gameObject.GetComponent<Food>().GetEnergy();
                target.gameObject.GetComponent<Food>().Consume();
                target = null;
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

        if(nearestFood != null && shortestDistance <= range){
            target = nearestFood.transform;
            this.nextPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            TurnAround();
            actualState = State.Moving;
        }else{
            target = null;
        }
    }

    void Reproduce(){
        creatureSpawnerInstance.SpawnCreature(transform.position);
        energy = energy - reproduceCost;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void die(){
        Destroy(gameObject);
    }
}
