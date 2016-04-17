using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evo.Core.Basic;
using Evo.Core.Universe;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class DecisionMakerTests
    {
        private DecisionMaker _decisionMaker;

        [SetUp]
        public void Init()
        {
            var world = new World(new Random(), new Coord(100, 100));
            world.EatDecisionModificator.Value = 1;
            world.SexDecisionModificator.Value = 1;
            world.KillDecisionModificator.Value = 1;
            _decisionMaker = new DecisionMaker(world);
        }

        [Test, Combinatorial]
        public void TestWantEat(
            [Values(1, 500, 1000)] int curEnery,
            [Values(1, 0.5, 10, 1000)] double distance)
        {
            double metric = _decisionMaker.WantEat(curEnery, 1000, distance);
            Assert.IsTrue(metric * distance >= 0);
            Assert.IsTrue(metric * distance <= 1);
        }

        [Test, Combinatorial]
        public void TestWantSex(
            [Values(0, 0.5, 1)] double desire,
            [Values(1, 0.5, 10, 1000)] double distance,
            [Values(0, 0.5, 1)] double difference)
        {
            double metric = _decisionMaker.WantSex(desire, distance, difference);
            Assert.IsTrue(metric * distance >= 0);
            Assert.IsTrue(metric * distance <= 1);
        }

        [Test, Combinatorial]
        public void TestWantKill(
            [Values(0, 0.5, 1)] double aggression,
            [Values(1, 0.5, 10, 1000)] double distance,
            [Values(0, 0.5, 1)] double difference,
            [Values(0, 0.5, 1)] double strengthQuotient)
        {
            double metric = _decisionMaker.WantKill(aggression, distance, difference, strengthQuotient);
            Assert.IsTrue(metric * distance >= 0);
            Assert.IsTrue(metric * distance <= 1);
        }
    }
}
