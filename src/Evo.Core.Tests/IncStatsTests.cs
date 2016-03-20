using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evo.Core.Stats;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class IncStatsTests
    {
        [Test]
        public void SimpleTest()
        {
            var stats = new IncStats();

            stats.AddStat("a", 3);
            stats.AddStat("b", 5);
            stats.AddStat("a", 6);

            Assert.AreEqual(9, stats.GetStat("a"));
            Assert.AreEqual(5, stats.GetStat("b"));
            Assert.AreEqual(0, stats.GetStat("c"));
        }
    }
}
