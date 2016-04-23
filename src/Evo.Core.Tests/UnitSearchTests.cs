using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evo.Core.Basic;
using Evo.Core.Units;
using Evo.Core.Universe;
using NUnit.Framework;

namespace Evo.Core.Tests
{
    [TestFixture]
    public class UnitSearchTests
    {
        [Test]
        public void SearchInListOf1()
        {
            var world = new World(new Random(0), new Coord(10, 10));
            var individual = new Individual(0, world);
            world.AddIndividual(individual);
            var foundIndividual = world.FindIndividualById(0);
            Assert.AreEqual(individual, foundIndividual);
        }

        [Test]
        public void SearchInListOf2()
        {
            var world = new World(new Random(0), new Coord(10, 10));
            var individual1 = new Individual(0, world);
            var individual2 = new Individual(2, world) { Point = new Coord(1, 1) };
            world.AddIndividual(individual1);
            world.AddIndividual(individual2);
            var foundIndividual1 = world.FindIndividualById(0);
            var foundIndividual2 = world.FindIndividualById(2);
            Assert.AreEqual(individual1, foundIndividual1);
            Assert.AreEqual(individual2, foundIndividual2);
        }

        [Test]
        public void SearchInListOf2NotFound()
        {
            var world = new World(new Random(0), new Coord(10, 10));
            var food1 = new FoodItem(0);
            var food2 = new FoodItem(2) { Point = new Coord(1, 1) };
            world.AddFood(food1);
            world.AddFood(food2);
            var foundFood = world.FindFoodById(150);
            Assert.IsNull(foundFood);
        }

        [Test]
        public void SearchInListOf5()
        {
            var world = new World(new Random(0), new Coord(10, 10));
            world.AddIndividual(new Individual(0, world));
            world.AddIndividual(new Individual(2, world) { Point = new Coord(1, 1) });
            world.AddIndividual(new Individual(3, world) { Point = new Coord(2, 1) });
            world.AddIndividual(new Individual(7, world) { Point = new Coord(3, 1) });
            world.AddIndividual(new Individual(98, world) { Point = new Coord(4, 1) });
            var foundIndividual = world.FindIndividualById(7);
            Assert.AreEqual(7, foundIndividual.Id);
        }
    }
}
