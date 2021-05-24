using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FoodTest
    {
        private GameObject go = new GameObject("FoodTest");

        private Food food { get; set; }

        [SetUp]
        public void SetUp()
        {
            food = go.AddComponent<Food>();
        }


        [Test]
        public void GetEnergy_WithDefaultEnergy_ReturnsZero()
        {
            var energy = food.GetEnergy();

            Assert.That(energy, Is.EqualTo(0));
        }

        [Test]
        public void GetEnergy_WithChangedEnergy_ReturnsPassedEnergy()
        {
            food.energy = 20f;

            var energy = food.GetEnergy();

            Assert.That(energy, Is.EqualTo(20f));
        }
    }
}
