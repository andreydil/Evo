﻿using Evo.Core.Basic;
using Evo.Core.Stats;
using Evo.Core.Units;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class StatCounterTests
    {
        [Test]
        public void SimpleTest()
        {
            var statCounter = new StatCounter();
            var average = statCounter.GetAverage(new[]
            {
                new Individual(0, null)
                {
                    Color = new ColorGene { Value = 0x102030 },
                    Aggression = new Gene(0, 10000) { Value = 200 },
                },
                new Individual(0, null)
                {
                    Color = new ColorGene { Value = 0x306050 },
                    Aggression = new Gene(0, 10000) { Value = 400 },
                },
            });

            Assert.AreEqual(300, average.Aggression);
            Assert.AreEqual(0x204040, average.Color);
        }

        [Test]
        public void GetEuclideanDistanceTest()
        {
            var statCounter = new StatCounter();
            var individual1 = new Individual(0, null);
            var individual2 = new Individual(0, null);
            individual2.Fertility.Value = individual2.Fertility + 1;

            Assert.AreEqual(1, statCounter.GetEuclideanDistance(individual1, individual2));
        }

        [Test]
        public void GetEuclideanDistanceWithColorTest()
        {
            var statCounter = new StatCounter();
            var individual1 = new Individual(0, null);
            var individual2 = new Individual(0, null);
            individual2.Color.Value = individual2.Color + 0x100;

            Assert.AreEqual(1, statCounter.GetEuclideanDistance(individual1, individual2));
        }
    }
}
