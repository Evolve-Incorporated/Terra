using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Generation
{

    public List<DNA> DNAList = new List<DNA>();
    public List<DNA> survivorsDNA = new List<DNA>();
    public int number = 0;
    public float startTime;

    public int totalAlive = 0;
    Generation prevGeneration = null;


    public Generation() {}

    public Generation(List<DNA> dnalist) {
        DNAList = dnalist;
    }

    public Generation(Generation generation) {
        number = generation.number + 1;
        prevGeneration = generation;
    }

    public bool IsAlive() {
        return totalAlive != 0;
    }

    public float TimeElapsed() {
        // if (startTime == null) throw
        return Time.time - startTime;
    }

    public void SetStartTime(float time = -1) {
        startTime = time != -1 ? time : Time.time; 
    }

    public void Run() {
        Debug.Log("Starting generation " + number);
        LoadDNAList();
        if (number != 0) {
            Selection();
            Reproduction();
        }
        fillDNAList();
        ResetDNAScores();
        SpawnCreatures();
        SetStartTime();
    }


    public List<DNA> GetSortedDNAList() {
        return DNAList.OrderByDescending(dna => dna.score).ToList();
    }


    public void FilterZeroDNAs() {
        DNAList = DNAList.Where(dna => dna.score > 0).ToList();
    }

    public void fillDNAList() {
        int count = DNAList.Count;
        for (int i = 0; i < CreatureSpawner.instance.creaturesCount - count; i++) DNAList.Add(DNA.RandomDNA());
        Debug.Log("Added random DNAs. [" + DNAList.Count + "/" + CreatureSpawner.instance.creaturesCount +  "]");
    }

    public void LoadDNAList() {
        if (prevGeneration != null) {
            if (prevGeneration.survivorsDNA.Count > 0) {
                DNAList = prevGeneration.survivorsDNA;
                Debug.Log("Selected " + DNAList.Count + " survivor DNAs to pass to next generation. Best score was: " + prevGeneration.GetSortedDNAList()[0].score);
            }
        }

        
        if (DNAList.Count == 0) {
            // if all creatures of previous generation died, take DNAs of the best dead creatures from previous generation and randomize the rest
            int halfMaxCount = (int)(CreatureSpawner.instance.creaturesCount * 0.5);

            if (prevGeneration != null) {
                if (prevGeneration.DNAList.Count > halfMaxCount) {
                    DNAList.AddRange(prevGeneration.DNAList.GetRange(0, halfMaxCount));
                } else {
                    DNAList.AddRange(prevGeneration.DNAList);
                    
                }
                Debug.Log("No creatures survived previous generation... Added  best dead DNAs from previous generation. [" + DNAList.Count + "/" + CreatureSpawner.instance.creaturesCount +  "]");
            }
        }
    }

    public void ResetDNAScores() {
        foreach(DNA dna in DNAList) {
            dna.ResetScore();
        }
    }

    public void Reproduction(bool mutate = true) {
        List<DNA> reproducedDNAs = new List<DNA>();
        if (DNAList.Count < 2 && DNAList.Count > 0) { // if one is present, reproduce by itself and mutate
            DNA reproducedDNA = new DNA(DNAList[0]); 
            if (mutate && UnityEngine.Random.Range(0f, 1f) >= GeneticAlgorithm.instance.mutationRate) reproducedDNA.Mutate(); 
            reproducedDNAs.Add(reproducedDNA);
        } else if (DNAList.Count >= 2) {
            for (int i = 0; i < DNAList.Count - 1; i++) {
                DNA parent1DNA = DNAList[i];
                DNA parent2DNA = DNAList[i + 1];
                List<DNA> crossedDNAs = parent1DNA.CrossoverBothWays(parent2DNA);
                foreach (DNA crossedDNA in crossedDNAs) crossedDNA.Mutate();
                reproducedDNAs.AddRange(crossedDNAs);
            }
        }
        DNAList.AddRange(reproducedDNAs);
        Debug.Log("Added " + reproducedDNAs.Count + " Offsprings. [" + DNAList.Count + "/" + CreatureSpawner.instance.creaturesCount +  "]");
    }

    public void Selection() {
        if (DNAList.Count < GeneticAlgorithm.instance.minSelectionCount) return;

        int selectionSize = (int) Mathf.Ceil(DNAList.Count * GeneticAlgorithm.instance.percentToPassSelection);
        if (selectionSize <= 0) {
            throw new IndexOutOfRangeException("Something went wrong..");
        }
        // if halfSize < minSize: add more creatures
        DNAList = DNAList.GetRange(0, selectionSize);
    }

    public void SpawnCreatures() {
        foreach(DNA dna in DNAList) CreatureSpawner.instance.SpawnCreature(dna, true);
        totalAlive = DNAList.Count;
    }

    public void End() {
        Debug.Log("Ending generation " + number);
        foreach(Creature creature in GameObject.FindObjectsOfType<Creature>()) {
            DNA dna = creature.GetDNA();
            if (dna.score > 0) survivorsDNA.Add(dna);
            creature.die();
            prevGeneration = null; // chyba performance boost
        }
        FilterZeroDNAs();
        DNAList = GetSortedDNAList();
    }

    public Generation Next() {
        Generation newGeneration = new Generation(this);
        return newGeneration;
    }

    void LogInfo() {
        
    }
}
