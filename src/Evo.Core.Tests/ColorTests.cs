using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evo.Core.Basic;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class ColorTests
    {
        private const double Tolerance = 0.0001;

        [Test]
        public void SimpleTest1()
        {
            var color1 = new ColorGene { Value = 1 };
            var color2 = new ColorGene { Value = 0 };

            Assert.That(color1.GetDifferenceFrom(color2), Is.EqualTo(1.0).Within(Tolerance));
        }

        [Test]
        public void SimpleTest2()
        {
            var color1 = new ColorGene { Value = 0 };
            var color2 = new ColorGene { Value = 0x030004 };

            Assert.That(color1.GetDifferenceFrom(color2), Is.EqualTo(5.0).Within(Tolerance));
        }

        [Test]
        public void SimpleTest3()
        {
            var color1 = new ColorGene { Value = 123 };
            var color2 = new ColorGene { Value = 123 };

            Assert.That(color1.GetDifferenceFrom(color2), Is.EqualTo(0.0).Within(Tolerance));
        }
    }
}
