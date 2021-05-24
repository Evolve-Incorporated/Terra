using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNABuilder
{
    private IDictionary<string, float> _values = new Dictionary<string, float>();

    public DNABuilder addEnergy(float value)
    {
        _values["energy"] = value;

        return this;
    }

    public DNABuilder addSpeed(float value)
    {
        _values["speed"] = value;

        return this;
    }

    public DNABuilder addRange(float value)
    {
        _values["range"] = value;

        return this;
    }

    public DNABuilder addReproductionCost(float value)
    {
        _values["reproductionCost"] = value;

        return this;
    }

    public DNABuilder addMaxEnergy(float value)
    {
        _values["maxEnergy"] = value;

        return this;
    }

    public DNA build()
    {
        var dna = new DNA();

        foreach (var kv in _values)
        {
            dna.SetGene(kv.Key, kv.Value);
        }

        return dna;
    }
}
