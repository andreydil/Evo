using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evo.Core.Basic;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class LimitedIntTests
    {
        [Test]
        public void MinTest()
        {
            var gene = new LimitedInt(1, 10);
            gene.Value = -2;
            Assert.AreEqual(1, gene.Value);
            gene.Value--;
            Assert.AreEqual(1, gene.Value);
        }

        [Test]
        public void MaxTest()
        {
            var gene = new LimitedInt(1, 10);
            gene.Value = 561;
            Assert.AreEqual(10, gene.Value);
            gene.Value++;
            Assert.AreEqual(10, gene.Value);
        }

        [Test]
        public void PlusOperatorTest()
        {
            var gene1 = new LimitedInt(0, 10) { Value = 4 };
            var gene2 = new LimitedInt(0, 10) { Value = 1 };
            Assert.AreEqual(5, gene1 + gene2);
        }

        [Test]
        public void MinusOperatorTest()
        {
            var gene1 = new LimitedInt(0, 10) { Value = 4 };
            var gene2 = new LimitedInt(0, 10) { Value = 1 };
            Assert.AreEqual(3, gene1 - gene2);
        }
    }
}
