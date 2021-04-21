using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class DNA  {

    
    public static Dictionary<string, float> DNA_RANGES = new Dictionary<string, float>() // to są wartości minimalne i maksymalne genów z których będzie losowane
    {
        // { "maxEnergy", new float[] {0f, 1000f} }, // max energia jaką bedzie miał stworek, na początku jak się go stworzy trzeba bedzie przypisać jego energii tą wartość
        // { "speed", new float[] {0f, 10f} }, 
        // { "range", new float[] {0f, 10f} },
        // { "reproductionCost", new float[] {0f, 1f} }, // to będzie w formie procentu maxEnergy, czyli np jesli stworek ma max energy 100 a reproductionCost 0.5 to bedzie musial miec 50 energii zeby sie rozmnozyc
        
        
        //key, maxValue
        { "maxEnergy", 5f }, // max energia jaką bedzie miał stworek, na początku jak się go stworzy trzeba bedzie przypisać jego energii tą wartość
        { "speed", 10f }, 
        { "range", 10f },
        { "reproductionCost", 1f},
        { "mutationRange", 0.1f}
    };

    [SerializeField]
    public static float reproductionDiscount = 0.3f;

    public Dictionary<string, float> genes = new Dictionary<string, float>();
    public float score = 0;    

    public static void MutateCreatureDNA(Creature creature) { // mozna to zmienic na np. MutateCreatureDNA(DNA dna), tak samo inne statyczne
        creature.GetDNA().Mutate();
    }

    public static void RandomizeCreatureDNA(Creature creature) {
        creature.GetDNA().SetGenes();
    }

    public static void SetCreatureDNA(Creature creature, DNA dna) {
        creature.GetDNA().SetGenes(dna.getGenes());
    }

 
    public static DNA RandomDNA() { 
        return (new DNA()).SetGenes();
    }


    public DNA() {}
    public DNA(DNA sourceDNA) {
        genes = new Dictionary<string, float>();
        SetGenes(sourceDNA.getGenes());
    }

    public void ResetScore() {
        score = 0;
    }

    public float getGene(string gene) {
        return genes[gene] * DNA_RANGES[gene];
    }

    public void SetGene(string gene, float value) {
        Debug.Log("Set speed to " + value);
        genes[gene] = value;
    }

    public float GetReproductionCost() {
        return getGene("maxEnergy") * getGene("reproductionCost");
    }

    public float GetReproductionCostAfterDiscount() {
        return GetReproductionCost() * (1 - reproductionDiscount);
    }

    public float GetMoveCost() {
        return 0.5f * (getGene("maxEnergy")*0.5f + getGene("speed")*0.2f + getGene("range")*0.3f);// / getGene("reproductionCost"); // tu trzeba uwzglednic jescze jakos maxEnergy bo jak to nie przyczynia sie do zwiekszenia kosztu to kazdy stworek bedzie dazyl do maksymalnej wartosci
    }

    public Dictionary<string, float> getGenes() {
        return genes;
    }

    public void Mutate() {
        // Narazie wszystko mutuje, potem mozna zrobic tak ze losowe geny mutują


        List<string> genesToMutate = (new List<string>(genes.Keys))
                                        .OrderBy(x => UnityEngine.Random.Range(0f, 1f))
                                        .Take( (int) UnityEngine.Random.Range(0f, (float) Mathf.Ceil(genes.Keys.Count * 0.51f)) )
                                        .ToList();


        //string info = "Mutating : " + String.Join(", ", genesToMutate);
        foreach (string key in genesToMutate) {
            //float[] range = DNA_RANGES[key];

            float mutation = genes[key] * ( 1 + (getGene("mutationRange") * UnityEngine.Random.Range(0,2)*2-1));
            //info += "\n" + "    Muating: " + key + "From: " + genes[key].ToString() + " To: " +  (genes[key] + mutation);
            genes[key] = Mathf.Clamp(mutation, 0f, 1f); // tymczasowe rozwiazanie zeby np reproduction cost nie spadlo do 0, nie wiem czy sposob obliczania kosztu energii podczas Move() jest na tyle ogarniety zeby nie dopuscic do takiej sytuacji
        }
        //Debug.Log(info + "\n");
    }

    public Dictionary<string, float> Clone() {
        Dictionary<string, float> newGenes = new Dictionary<string, float>();
        foreach (KeyValuePair<string, float> entry in genes){
            newGenes[entry.Key] = entry.Value;
        }
        return newGenes;
    }

    private DNA SetGenes(){
        foreach (KeyValuePair<string, float> entry in DNA.DNA_RANGES){
            float value = UnityEngine.Random.Range(0f, 1f);
            genes[entry.Key] = value;
        }
        return this;
    }


    private DNA SetGenes(Dictionary<string, float> sourceGenes) {
        foreach (KeyValuePair<string, float> entry in sourceGenes){
            genes[entry.Key] = entry.Value;
        }
        return this;
    }

    public override string ToString() {
        string text = "";
        foreach (KeyValuePair<string, float> entry in genes){
            text += entry.Key + ": " + entry.Value + "\n";
        }
        return text;
    }

}