using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DNATest
    {
        private DNA dna { get; set; }

        [SetUp]
        public void SetUp()
        {
            dna = new DNA();
        }

        [Test]
        public void ResetScore_CallWithScoreZero_DoesNothing()
        {
            dna.ResetScore();

            Assert.That(dna.score, Is.EqualTo(0));
        }

        [Test]
        public void ResetScore_CallWithScoreNonZero_ResetsScore()
        {
            dna.score = 5;

            dna.ResetScore();

            Assert.That(dna.score, Is.EqualTo(0));
        }

        [Test]
        public void GetReproductionCost_WithTwoZeros_ReturnZero()
        {
            dna.SetGene("maxEnergy", 0f);
            dna.SetGene("reproductionCost", 0f);

            var cost = dna.GetReproductionCost();

            Assert.That(cost, Is.EqualTo(0));
        }

        [Test]
        public void GetReproductionCost_WithTwoValues_ReturnMultiplication()
        {
            dna.SetGene("maxEnergy", 5f);
            dna.SetGene("reproductionCost", 3f);

            var cost = dna.GetReproductionCost();

            Assert.That(cost, Is.EqualTo(5 * 3 * 5 /* DNA_RANGES[maxEnergy] */));
        }

        [Test]
        public void SetGene_WithAlreadyExistingGene_OverridesGenesValue()
        {
            dna.SetGene("abc", 1f);

            dna.SetGene("abc", 5f);

            Assert.That(dna.getGene("abc"), Is.EqualTo(5f));
        }

        [Test]
        public void getGenes_WithNoGenes_ReturnsEmptyCollection()
        {
            var genes = dna.getGenes();

            Assert.That(genes.Count, Is.EqualTo(0));
        }

        [Test]
        public void getGenes_WithGenes_ReturnsCollectionOfGenes()
        {
            dna.SetGene("abc", 1f);
            dna.SetGene("abcd", 1f);
            dna.SetGene("abce", 1f);

            var genes = dna.getGenes();

            Assert.That(genes.Count, Is.EqualTo(3));
            Assert.That(genes["abc"], Is.EqualTo(1f));
            Assert.That(genes["abcd"], Is.EqualTo(1f));
            Assert.That(genes["abce"], Is.EqualTo(1f));
        }
    }

}
