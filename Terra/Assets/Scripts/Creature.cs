using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    enum State {Waiting, Moving}
    private State actualState;
    private Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        actualState = State.Waiting;
    }

    // Update is called once per frame
    void Update()
    {
        if(actualState == State.Waiting ){
            NextPosition();
            actualState = State.Moving;
        }else if(actualState == State.Moving){
            Move();
        }

    }

    void NextPosition(){
        float xPositionRandom = Random.Range(-5f, 5f);
        float yPositionRandom = Random.Range(-5f, 5f);
        this.nextPosition = new Vector3(transform.position.x + xPositionRandom, transform.position.y + yPositionRandom, transform.position.z);
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
