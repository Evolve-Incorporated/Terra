using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public void Pause(){
        Time.timeScale = 0f;
    }

    public void Play(){
        Time.timeScale = 1f;
    }

    public void IncreaseSpeed(){
        Time.timeScale = 10f;
    }

    public void TopSpeed(){
        Time.timeScale = 30f;
    }
}
