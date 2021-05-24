using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float CurrentScale { get; private set; }

    public void Pause(){
        CurrentScale = 0f;
        Time.timeScale = 0f;
    }

    public void Play(){
        CurrentScale = 1f;
        Time.timeScale = 1f;
    }

    public void IncreaseSpeed(){
        CurrentScale = 10f;
        Time.timeScale = 10f;
    }

    public void TopSpeed(){
        CurrentScale = 30f;
        Time.timeScale = 30f;
    }
}
