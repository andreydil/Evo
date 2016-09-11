using System;
using System.Collections.Generic;
using Evo.Core.Basic;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    public class DefaultWorldConfigurator : IWorldConfigurator
    {
        private Coord Size { get; set; }

        public DefaultWorldConfigurator(int width, int height)
        {
            Size = new Coord(width, height);
        }

        public World CreateWorld()
        {
            var world = new World(new Random(0), Size);
            world.MutationProbability.Value = 120;
            world.MutationMaxDelta.Value = 40;
            world.EnergyDrainModificator.Value = 1;
            world.MaxFoodItemsPerTick.Value = 60;
            world.MaxEneryPerFoodItem.Value = 140;
            world.MaxFoodItems.Value = 400;
            world.BirthEnergyShare.Value = 30;

            var initPopulation = new List<Individual>(100);
            for (int i = 0; i < 50; i++)
            {
                var individual = world.Mutator.GenerateAverage();
                initPopulation.Add(individual);
            }
            world.SpreadIndividuals(initPopulation, new Coord(0, 0), Size);
            world.SpreadFood();
            return world;
        }
    }
}
