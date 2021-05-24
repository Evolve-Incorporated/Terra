using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    public GameObject map;

    public void ExitGame(){
        Debug.Log("exit game");
        Application.Quit();
    }

    public void StartSimulation(){
        Debug.Log("start simuulation");
        Instantiate(map, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(gameObject);
    }
}
