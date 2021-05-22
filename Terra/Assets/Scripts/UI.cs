using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public void ExitGame(){
        Application.Quit();
    }

    public void StartSimulation(){
        SceneManager.LoadScene("SampleScene");
    }
}
