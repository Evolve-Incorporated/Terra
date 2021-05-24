using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CreatureTest
    {
        private GameObject go = new GameObject("CreatureTest");

        private Creature creature { get; set; }

        [SetUp]
        public void SetUp()
        {
            creature = go.AddComponent<Creature>();
            creature.dna = new DNA();
            creature.dna.SetGene("maxEnergy", 10f);
            creature.energy = 0f;
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommitEnergy_PassZero_DnaScoreIncrement()
        {
            Assert.That(creature.dna.score, Is.EqualTo(0f));
        }

        [Test]
        public void CommitEnergy_PassNonZero_EnergyIncreased()
        {
            creature.CommitEnergy(5f);

            Assert.That(creature.energy, Is.EqualTo(5f));
        }

        [Test]
        public void BurnEnergy_PassZero_EnergyNotChanged()
        {
            creature.CommitEnergy(5f);

            creature.BurnEnergy(0f);

            Assert.That(creature.energy, Is.EqualTo(5f));
        }

        [Test]
        public void BurnEnergy_PassNonZero_EnergyChanged()
        {
            creature.CommitEnergy(5f);

            creature.BurnEnergy(2f);

            Assert.That(creature.energy, Is.EqualTo(5f - 2f));
        }
    }
}
