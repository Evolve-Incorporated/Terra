using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureFactory
{
    private static Lazy<CreatureFactory> _instance = new Lazy<CreatureFactory>(() => new CreatureFactory());

    private CreatureFactory() { }

    public static CreatureFactory Instance => _instance.Value;

    public Creature CreateCreature()
    {
        var dna = new DNA();

        var creature = new Creature();

        creature.dna = dna;

        return creature;
    }
}
