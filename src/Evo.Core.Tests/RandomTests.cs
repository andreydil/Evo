using System;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class RandomTests
    {
        [Test]
        public void SeededRandomTest()
        {
            var rng = new Random(1);
            var val1 = rng.Next();
            var val2 = rng.Next(10);
            Assert.AreEqual(534011718, val1);
            Assert.AreEqual(1, val2);
        }
    }
}
