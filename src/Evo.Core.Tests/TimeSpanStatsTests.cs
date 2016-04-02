using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evo.Core.Stats;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class TimeSpanStatsTests
    {
        [Test]
        public void OverflowTest()
        {
            var stats = new TimeSpanStats<int>(3);
            stats.Add(1, 1);
            stats.Add(2, 2);
            stats.Add(3, 3);
            stats.Add(4, 4);

            Assert.AreEqual(3, stats.Count());
            Assert.AreEqual(2, stats[0].Tick);
            Assert.AreEqual(3, stats[1].Value);
            Assert.AreEqual(4, stats[2].Tick);
        }
    }
}
