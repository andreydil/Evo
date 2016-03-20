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
    public class CoordTests
    {
        [Test]
        public void EqualsTest()
        {
            var p1 = new Coord(2, 5);
            var p2 = new Coord(2, 7);
            var p3 = new Coord(2, 7);

            Assert.IsFalse(p1 == p2);
            Assert.IsTrue(p1 != p2);
            Assert.IsTrue(p2 == p3);
            Assert.IsFalse(p1.Equals(p2));
            Assert.IsTrue(p2.Equals(p3));
        }

        [Test]
        public void PlusTest()
        {
            var p1 = new Coord(5, 2);
            var p2 = new Coord(1, -4);

            var p3 = p1 + p2;
            Assert.AreEqual(6, p3.X);
            Assert.AreEqual(-2, p3.Y);
        }

        [Test]
        public void MinusTest()
        {
            var p1 = new Coord(5, 2);
            var p2 = new Coord(1, 3);

            var p3 = p1 - p2;
            Assert.AreEqual(4, p3.X);
            Assert.AreEqual(-1, p3.Y);
        }

        [Test]
        public void GetDistanceTest()
        {
            var p1 = new Coord(0, 4);
            var p2 = new Coord(3, 0);

            var distance = p1.GetDistanceTo(p2);
            Assert.IsTrue(Math.Abs(5 - distance) < 0.0001);
        }

        [Test]
        public void FindDirectionTest1()
        {
            var p1 = new Coord(50, 50);
            var p2 = new Coord(43, 51);
            var direction = p1.FindDirection(p2);
            Assert.AreEqual(-1, direction.X);
            Assert.AreEqual(1, direction.Y);
        }

        [Test]
        public void FindDirectionTest2()
        {
            var p1 = new Coord(50, 50);
            var p2 = new Coord(63, 50);
            var direction = p1.FindDirection(p2);
            Assert.AreEqual(1, direction.X);
            Assert.AreEqual(0, direction.Y);
        }
    }
}
