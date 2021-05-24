using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public GameObject game;
    public Slider MapSizeSlider;
    public Slider PopulationCountSlider;
    public Slider FoodCountSlider;

    public void ExitGame(){
        Application.Quit();
    }

    public void StartSimulation(){
        GameObject map = game.transform.GetChild(1).gameObject;
        map.GetComponent<Map>().setSize((int)MapSizeSlider.value);
        map.GetComponent<FoodSpawner>().setCount((int)PopulationCountSlider.value);
        map.GetComponent<CreatureSpawner>().setCount((int)FoodCountSlider.value);
        Instantiate(game, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(gameObject);
    }
}
