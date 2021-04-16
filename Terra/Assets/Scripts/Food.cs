using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    [SerializeField]
    private float energy;
    [SerializeField]
    private float probability;

    public void Consume(){
        Destroy(gameObject);
    }

    public float GetEnergy(){
        return energy;
    }
}
