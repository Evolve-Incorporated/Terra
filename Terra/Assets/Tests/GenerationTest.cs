using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GenerationTest
    {
        private Generation generation { get; set; }

        [SetUp]
        public void SetUp()
        {
            generation = new Generation();
        }


        [Test]
        public void ResetDNAScores_WithEmptyList_DoesNotEditDNAList()
        {
            generation.ResetDNAScores();

            Assert.That(generation.DNAList.Count, Is.EqualTo(0));
        }

        [Test]
        public void ResetDNAScores_WithNonEmptyList_ResetsScores()
        {
            var dna = new DNA();
            dna.score = 5f;

            generation.DNAList.Add(dna);

            generation.ResetDNAScores();

            Assert.That(generation.DNAList[0].score, Is.EqualTo(0));
        }

        [Test]
        public void IsAlive_NoCreatureAlive_ReturnsFalse()
        {
            generation.totalAlive = 0;

            Assert.That(generation.IsAlive(), Is.False);
        }

        [Test]
        public void IsAlive_SomeCreatureAlive_ReturnsTrue()
        {
            generation.totalAlive = 5;

            Assert.That(generation.IsAlive(), Is.True);
        }
    }
}
