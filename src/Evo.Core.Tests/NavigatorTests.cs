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
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void PutUnitTwiceInTheSamePlace()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var individual = new Individual(1, world)
            {
                Point = new Coord(5, 5),
            };
            world.Navigator.PutUnit(individual);
            world.Navigator.PutUnit(individual);
        }

        [Test]
        public void FoodFindingTest1()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            world.AddFood(new FoodItem(1)
            {
                Point = new Coord(43, 51),
            });
            world.AddFood(new FoodItem(2)
            {
                Point = new Coord(53, 52),
            });
            world.AddFood(new FoodItem(3)
            {
                Point = new Coord(50, 50),
            });
            var food = world.Navigator.FindClosestFood(new Coord(50, 50), 15);
            Assert.IsNotNull(food);
            Assert.AreEqual(2, food.Id);
        }

        [Test]
        public void FoodFindingTest2()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            world.AddFood(new FoodItem(1)
            {
                Point = new Coord(0, 0),
            });
            world.AddFood(new FoodItem(2)
            {
                Point = new Coord(1, 2),
            });
            var food = world.Navigator.FindClosestFood(new Coord(0, 0), 5);
            Assert.IsNotNull(food);
            Assert.AreEqual(2, food.Id);
        }

        [Test]
        public void BounceFromBordersTest1()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var point = new Coord(50, 30);
            var direction = new Coord(1, -1);
            direction = world.Navigator.BounceFromWalls(point, direction);
            Assert.AreEqual(1, direction.X);
            Assert.AreEqual(-1, direction.Y);
        }

        [Test]
        public void BounceFromBordersTest2()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var point = new Coord(0, 0);
            var direction = new Coord(-1, -1);
            direction = world.Navigator.BounceFromWalls(point, direction);
            Assert.AreEqual(1, direction.X);
            Assert.AreEqual(1, direction.Y);
        }

        [Test]
        public void BounceFromBordersTest3()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var point = new Coord(world.Size.X, world.Size.Y);
            var direction = new Coord(1, -1);
            direction = world.Navigator.BounceFromWalls(point, direction);
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

        [Test]
        public void FindUnitNotBehindAWallTest1()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var behindWall = new Individual(1, world) { Point = new Coord(5, 1) };
            var notBehindWall = new Individual(2, world) { Point = new Coord(2, 8) };
            world.AddIndividuals(new[] { behindWall, notBehindWall });
            world.AddWall(new Wall(WallType.Vertical, 4));
            var target = world.Navigator.FindClosestIndividual(new Coord(2, 2), 10);
            Assert.AreEqual(2, target.Point.X);
            Assert.AreEqual(8, target.Point.Y);
        }

        [Test]
        public void FindUnitNotBehindAWallTest2()
        {
            var world = new World(new Random(0), new Coord(100, 100));
            var behindWall = new Individual(1, world) { Point = new Coord(2, 8) };
            var notBehindWall = new Individual(2, world) { Point = new Coord(5, 1) };
            world.AddIndividuals(new[] { behindWall, notBehindWall });
            world.AddWall(new Wall(WallType.Vertical, 4));
            var target = world.Navigator.FindClosestIndividual(new Coord(5, 9), 10);
            Assert.AreEqual(5, target.Point.X);
            Assert.AreEqual(1, target.Point.Y);
        }

        [Test]
        public void IsWallTest()
        {
            var world = new World(new Random(0), new Coord(50, 50));
            world.AddWall(new Wall(WallType.Vertical, 4));
            Assert.IsTrue(world.Navigator.IsWall(new Coord(4, 2)));
            Assert.IsFalse(world.Navigator.IsWall(new Coord(5, 2)));
        }

        [Test]
        public void EnsureTopLeftPointWallsTest1()
        {
            var world = new World(new Random(0), new Coord(50, 50));
            world.AddWall(new Wall(WallType.Vertical, 4));
            world.AddWall(new Wall(WallType.Horizontal, 3));
            var pointFrom = new Coord(6, 6);
            var topLeft = world.Navigator.EnsureTopLeftPointWalls(new Coord(0, 0), pointFrom);
            Assert.AreEqual(5, topLeft.X);
            Assert.AreEqual(4, topLeft.Y);
        }

        [Test]
        public void EnsureTopLeftPointWallsTest2()
        {
            var world = new World(new Random(0), new Coord(50, 50));
            world.AddWall(new Wall(WallType.Vertical, 5));
            var pointFrom = new Coord(6, 6);
            var topLeft = world.Navigator.EnsureTopLeftPointWalls(new Coord(0, 0), pointFrom);
            Assert.AreEqual(6, topLeft.X);
            Assert.AreEqual(0, topLeft.Y);
        }

        [Test]
        public void EnsureBottomRightWallsTest1()
        {
            var world = new World(new Random(0), new Coord(50, 50));
            world.AddWall(new Wall(WallType.Vertical, 4));
            world.AddWall(new Wall(WallType.Horizontal, 3));
            var pointFrom = new Coord(2, 2);
            var topLeft = world.Navigator.EnsureBottomRightPointWalls(new Coord(20, 160), pointFrom);
            Assert.AreEqual(3, topLeft.X);
            Assert.AreEqual(2, topLeft.Y);
        }

        [Test]
        public void EnsureBottomRightWallsTest2()
        {
            var world = new World(new Random(0), new Coord(50, 50));
            world.AddWall(new Wall(WallType.Horizontal, 3));
            var pointFrom = new Coord(2, 2);
            var topLeft = world.Navigator.EnsureBottomRightPointWalls(new Coord(20, 160), pointFrom);
            Assert.AreEqual(20, topLeft.X);
            Assert.AreEqual(2, topLeft.Y);
        }
    }
}
