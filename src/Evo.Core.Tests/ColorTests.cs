using Evo.Core.Basic;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class ColorTests
    {
        private const double Tolerance = 0.0001;

        [Test]
        public void SetRGBTest()
        {
            const byte red = 0x10;
            const byte green = 0x39;
            const byte blue = 0x8;

            var color = new ColorGene();
            color.SetRGB(red, green, blue);

            Assert.AreEqual(0x103908, color);
        }

        [Test]
        public void GetRGBTest()
        {
            var color = new ColorGene { Value = 0x103908 };

            Assert.AreEqual(0x10, color.Red);
            Assert.AreEqual(0x39, color.Green);
            Assert.AreEqual(0x08, color.Blue);
        }

        [Test]
        public void GetDifferenceTest1()
        {
            var color1 = new ColorGene { Value = 1 };
            var color2 = new ColorGene { Value = 0 };

            Assert.That(color1.GetDifferenceFrom(color2), Is.EqualTo(1.0).Within(Tolerance));
        }

        [Test]
        public void GetDifferenceTest2()
        {
            var color1 = new ColorGene { Value = 0 };
            var color2 = new ColorGene { Value = 0x030004 };

            Assert.That(color1.GetDifferenceFrom(color2), Is.EqualTo(5.0).Within(Tolerance));
        }

        [Test]
        public void GetDifferenceTest3()
        {
            var color1 = new ColorGene { Value = 123 };
            var color2 = new ColorGene { Value = 123 };

            Assert.That(color1.GetDifferenceFrom(color2), Is.EqualTo(0.0).Within(Tolerance));
        }
    }
}
