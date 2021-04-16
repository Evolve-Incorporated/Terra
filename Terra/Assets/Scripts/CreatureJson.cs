using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureJson
{
    private float speed;
    private float range;
    private float energy;
    private float reproduceCost;
    private float tendencyToReproduce;

    private static float minSpeed = 1f;
    private static float maxSpeed = 5f;

    private static float minRange = 1f;
    private static float maxRange = 3f;

    private static float minEnergy = 30f;
    private static float maxEnergy = 60f;

    private static float minReproduceCost = 70f;
    private static float maxReproduceCost = 100f;

    private static float minTendencyToReproduce = 1.1f;
    private static float maxTendencyToReproduce = 1.5f;

    public CreatureJson(float speed, float range, float energy, float reproduceCost, float tendencyToReproduce){
        this.speed = speed;
        this.range = range;
        this.energy = energy;
        this.reproduceCost = reproduceCost;
        this.tendencyToReproduce = tendencyToReproduce;
    }

    public static CreatureJson getRandom(){
        float speed = Random.Range(minSpeed, maxSpeed);
        float range = Random.Range(minRange, maxRange);
        float energy = Random.Range(minEnergy, maxEnergy);
        float reproduceCost = Random.Range(minReproduceCost, maxReproduceCost);
        float tendencyToReproduce = Random.Range(minTendencyToReproduce, maxTendencyToReproduce);
        return new CreatureJson(speed, range, energy, reproduceCost, tendencyToReproduce);
    }

    public float getSpeed(){
        return speed;
    }

    public float getRange(){
        return range;
    }

    public float getEnergy(){
        return energy;
    }

    public float getReproduceCost(){
        return reproduceCost;
    }

    public float getTendencyToReproduce(){
        return tendencyToReproduce;
    }
}
