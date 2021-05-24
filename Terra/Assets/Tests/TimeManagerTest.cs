using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TimeManagerTest
    {
        private GameObject go = new GameObject("TimeManagerTest");

        private TimeManager timeManager { get; set; }

        [SetUp]
        public void SetUp()
        {
            timeManager = go.AddComponent<TimeManager>();
        }

        [Test]
        public void Pause_WithDefaultScale_SetsScaleToZero()
        {
            timeManager.Pause();

            Assert.That(timeManager.CurrentScale, Is.EqualTo(0f));
        }

        [Test]
        public void Play_WithDefaultScale_SetsScaleToOne()
        {
            timeManager.Play();

            Assert.That(timeManager.CurrentScale, Is.EqualTo(1f));
        }

        [Test]
        public void IncreaseSpeed_WithDefaultScale_SetsScaleToTen()
        {
            timeManager.IncreaseSpeed();

            Assert.That(timeManager.CurrentScale, Is.EqualTo(10f));
        }

        [Test]
        public void TopSpeed_WithDefaultScale_SetsScaleToThirty()
        {
            timeManager.TopSpeed();

            Assert.That(timeManager.CurrentScale, Is.EqualTo(30f));
        }
    }
}
