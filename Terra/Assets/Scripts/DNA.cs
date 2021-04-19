using System.Collections.Generic;
using UnityEngine;

public class DNA  {

    [SerializeField]
    public static Dictionary<string, float[]> DNA_RANGES = new Dictionary<string, float[]>() // to są wartości minimalne i maksymalne genów z których będzie losowane
    {
        { "maxEnergy", new float[] {1f, 100f} }, // max energia jaką bedzie miał stworek, na początku jak się go stworzy trzeba bedzie przypisać jego energii tą wartość
        { "speed", new float[] {1f, 10f} }, 
        { "range", new float[] {0f, 10f} },
        { "reproductionCost", new float[] {0.8f, 1.0f} }, // to będzie w formie procentu maxEnergy, czyli np jesli stworek ma max energy 100 a reproductionCost 0.5 to bedzie musial miec 50 energii zeby sie rozmnozyc
    };

    [SerializeField]
    public static float mutationRange = 0.1f; // to tez moze byc genem

    public Dictionary<string, float> genes;
    

    public static void MutateCreatureDNA(Creature creature) { // mozna to zmienic na np. MutateCreatureDNA(DNA dna), tak samo inne statyczne
        creature.GetDNA().Mutate();
    }

    public static void RandomizeCreatureDNA(Creature creature) {
        creature.GetDNA().SetGenes();
    }

    public static void SetCreatureDNA(Creature creature, DNA dna) {
        creature.GetDNA().SetGenes(dna.getGenes());
    }

 
    public DNA() {
        genes = new Dictionary<string, float>();
    }

    public DNA(DNA sourceDNA) {
        genes = new Dictionary<string, float>();
        SetGenes(sourceDNA.getGenes());
    }

    public float getGene(string gene) {
        return genes[gene];
    }

    public float GetReproductionCost() {
        return getGene("maxEnergy") * getGene("reproductionCost");
    }

    public float GetMoveCost() {
        return 0.01f * (getGene("speed") + getGene("range")) / getGene("reproductionCost"); // tu trzeba uwzglednic jescze jakos maxEnergy bo jak to nie przyczynia sie do zwiekszenia kosztu to kazdy stworek bedzie dazyl do maksymalnej wartosci
    }

    public Dictionary<string, float> getGenes() {
        return genes;
    }

    public void Mutate() {
        // Narazie wszystko mutuje, potem mozna zrobic tak ze losowe geny mutują

        string info = "";
        foreach (string key in new List<string>(genes.Keys)) {
            float[] range = DNA_RANGES[key];
            float mutation = genes[key] * Random.Range(-mutationRange, mutationRange);
            info += "\n" + "    Muating: " + key + "From: " + genes[key].ToString() + " To: " +  (genes[key] + mutation);
            genes[key] = Mathf.Clamp(genes[key] + mutation, range[0], range[1]); // tymczasowe rozwiazanie zeby np reproduction cost nie spadlo do 0, nie wiem czy sposob obliczania kosztu energii podczas Move() jest na tyle ogarniety zeby nie dopuscic do takiej sytuacji
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

    private void SetGenes(){
        foreach (KeyValuePair<string, float[]> entry in DNA.DNA_RANGES){
            float value = Random.Range(entry.Value[0], entry.Value[1]);
            genes[entry.Key] = value;
        }
    }


    private void SetGenes(Dictionary<string, float> sourceGenes) {
        foreach (KeyValuePair<string, float> entry in sourceGenes){
            genes[entry.Key] = entry.Value;
        }
    }

    public override string ToString() {
        string text = "";
        foreach (KeyValuePair<string, float> entry in genes){
            text += entry.Key + ": " + entry.Value + "\n";
        }
        return text;
    }

}