using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Generation
{
    public static float percentToPassSelection = 0.5f;
    public static int maxDurationSeconds = 20;


    public List<DNA> DNAList = new List<DNA>();
    public List<DNA> survivorsDNA = new List<DNA>();
    public int number = 0;

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

    public void Run() {
        LoadDNAList();
        Debug.Log("Starting generation " + number + ". Number of DNAs: " + DNAList.Count);
        if (number != 0) {
            Selection();
            Reproduction();
        }
        ResetDNAScores();
        SpawnCreatures();
    }


    public List<DNA> GetSortedDNAList() {
        return DNAList.OrderByDescending(dna => dna.score).ToList();
    }

    public void FilterZeroDNAs() {
        DNAList = DNAList.Where(dna => dna.score > 0).ToList();
    }


    public void LoadDNAList() {
        if (prevGeneration != null) {
            prevGeneration.FilterZeroDNAs();
            DNAList = prevGeneration.survivorsDNA;
            if (prevGeneration.DNAList.Count > 0) Debug.Log("Previous Gen best score: " + prevGeneration.GetSortedDNAList()[0].score);
            else Debug.Log("All previous DNAs were dogshit");
        }
        Debug.Log("Selected " + DNAList.Count + " survivor DNAs to pass to next generation.");
        if (DNAList.Count == 0) {
            Debug.Log("No creatures in population...");
            // if all creatures of previous generation died, take DNAs of the best dead creatures from previous generation and randomize the rest
            int halfMaxCount = (int)(CreatureSpawner.instance.creaturesCount * 0.5);

            
            if (prevGeneration != null) {
                List<DNA> sorted = prevGeneration.GetSortedDNAList();
                if (sorted.Count > halfMaxCount) {
                    DNAList.AddRange(sorted.GetRange(0, halfMaxCount));
                    Debug.Log("Added " + halfMaxCount + " best DNAs from previous generation.");
                } else {
                    DNAList.AddRange(sorted);
                    Debug.Log("Added " + sorted.Count + " best DNAs from previous generation.");
                }
                
            }
        
            // Debug.Log("Added " + DNAList.Count + " best DNAs from past generation.");
            int count = DNAList.Count;
            for (int i = 0; i < CreatureSpawner.instance.creaturesCount - count; i++) DNAList.Add(DNA.RandomDNA());
            Debug.Log("Added " + (CreatureSpawner.instance.creaturesCount - count) + " random DNAs");
        }
    }

    public void ResetDNAScores() {
        foreach(DNA dna in DNAList) {
            dna.ResetScore();
        }
    }

    public void Reproduction(bool mutate = true) {
        foreach (DNA dna in new List<DNA>(DNAList)) {
            DNA reproducedDNA = new DNA(dna); //clone dna, this will be the DNA of a new creature
            if (mutate) reproducedDNA.Mutate(); //mutate dna
            DNAList.Add(reproducedDNA); //add new dna to pool
        }
    }

    public void Selection() {
        int selectionSize = (int) Mathf.Ceil(DNAList.Count * percentToPassSelection);
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
        foreach(Creature creature in GameObject.FindObjectsOfType<Creature>()) {
            DNA dna = creature.GetDNA();
            if (dna.score > 0) survivorsDNA.Add(dna);
            
            creature.die();
            prevGeneration = null; // chyba performance boost
        }
        
    }

    public Generation Next() {
        Generation newGeneration = new Generation(this);
        return newGeneration;
    }

    void LogInfo() {
        
    }
}
