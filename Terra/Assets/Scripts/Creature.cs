using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    public float range = 5f;

    enum State {Waiting, Moving}
    private State actualState;
    private Vector3 nextPosition;
    private Map mapInstance;
    private float mapOffset = 0.5f;
    private string foodTag = "Food";

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        actualState = State.Waiting;
        this.mapInstance = Map.instance;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
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
        }else{
            target = null;
        }
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
        }
    }

}
