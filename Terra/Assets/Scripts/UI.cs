using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public GameObject map;
    public Slider MapSizeSlider;
    public Slider PopulationCountSlider;
    public Slider FoodCountSlider;

    public void ExitGame(){
        Application.Quit();
    }

    public void StartSimulation(){
        this.map.GetComponent<Map>().setSize((int)MapSizeSlider.value);
        this.map.GetComponent<FoodSpawner>().setCount((int)PopulationCountSlider.value);
        this.map.GetComponent<CreatureSpawner>().setCount((int)FoodCountSlider.value);
        Instantiate(map, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(gameObject);
    }
}
