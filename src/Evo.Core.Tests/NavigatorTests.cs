using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evo.Core.Basic;
using Evo.Core.Units;
using Evo.Core.Universe;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class NavigatorTests
    {
        [Test]
        public void FoodFindingTest()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            world.Food.Clear();
            world.AddFood(new FoodItem(1)
            {
                Point = new Coord(43, 51),
            });
            world.AddFood(new FoodItem(2)
            {
                Point = new Coord(54, 53),
            });
            world.AddFood(new FoodItem(3)
            {
                Point = new Coord(50, 50),
            });
            var food = world.Navigator.FindClosestFood(new Coord(50, 50), 15);
            Assert.AreEqual(2, food.Id);
        }

        [Test]
        public void BounceFromBordersTest1()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var point = new Coord(50, 30);
            var direction = new Coord(1, -1);
            direction = world.Navigator.BounceFromMapBorders(point, direction);
            Assert.AreEqual(1, direction.X);
            Assert.AreEqual(-1, direction.Y);
        }

        [Test]
        public void BounceFromBordersTest2()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var point = new Coord(0, 0);
            var direction = new Coord(-1, -1);
            direction = world.Navigator.BounceFromMapBorders(point, direction);
            Assert.AreEqual(1, direction.X);
            Assert.AreEqual(1, direction.Y);
        }

        [Test]
        public void BounceFromBordersTest3()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var point = new Coord(world.Size.X, world.Size.Y);
            var direction = new Coord(1, -1);
            direction = world.Navigator.BounceFromMapBorders(point, direction);
            Assert.AreEqual(-1, direction.X);
            Assert.AreEqual(-1, direction.Y);
        }

        [Test]
        public void PlaceChildSimpleTest()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var father = new Individual(1, world) { Point = new Coord(1, 14) };
            var mother = new Individual(2, world) { Point = new Coord(2, 14) };
            world.AddIndividual(father);
            world.AddIndividual(mother);
            var childPoint = world.Navigator.PlaceChild(father.Point, mother.Point);
            Assert.IsNotNull(childPoint);
            Assert.AreEqual(1, childPoint.Value.X);
            Assert.AreEqual(13, childPoint.Value.Y);
        }

        [Test]
        public void PlaceChildInCrowdTest()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var father = new Individual(1, world) { Point = new Coord(2, 11) };
            var mother = new Individual(2, world) { Point = new Coord(3, 10) };
            world.AddIndividual(father);
            world.AddIndividual(mother);
            world.AddIndividuals(new[]
            {
                new Individual(3, world) { Point = new Coord(2, 9) },
                new Individual(4, world) { Point = new Coord(3, 9) },
                new Individual(5, world) { Point = new Coord(4, 9) },
                new Individual(6, world) { Point = new Coord(1, 10) },
                new Individual(7, world) { Point = new Coord(2, 10) },
                new Individual(8, world) { Point = new Coord(4, 10) },
                new Individual(9, world) { Point = new Coord(3, 11) },
                new Individual(10, world) { Point = new Coord(4, 11) },
            });
            var childPoint = world.Navigator.PlaceChild(father.Point, mother.Point);
            Assert.IsNotNull(childPoint);
            Assert.AreEqual(1, childPoint.Value.X);
            Assert.AreEqual(11, childPoint.Value.Y);
        }

        [Test]
        public void PlaceChildInCornerTest()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var father = new Individual(1, world) { Point = new Coord(0, 1) };
            var mother = new Individual(2, world) { Point = new Coord(0, 0) };
            world.AddIndividual(father);
            world.AddIndividual(mother);
            var childPoint = world.Navigator.PlaceChild(father.Point, mother.Point);
            Assert.IsNotNull(childPoint);
            Assert.AreEqual(1, childPoint.Value.X);
            Assert.AreEqual(0, childPoint.Value.Y);
        }
    }
}
