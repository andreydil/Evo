using System;
using System.Collections.Generic;
using Evo.Core.Basic;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    public class ParamsWorldConfigurator: IWorldConfigurator
    {
        public Coord Size { get; set; }
        public Random Random { get; set; } = new Random();
        public int InitPopulationSize { get; set; } = 50;
        public bool RandomIndividuals { get; set; } = false;

        public ParamsWorldConfigurator(int width, int height)
        {
            Size = new Coord(width, height);
        }

        public World CreateWorld()
        {
            var world = new World(Random, Size);
            world.MutationProbability.Value = 120;
            world.MutationMaxDelta.Value = 100;
            world.EnergyDrainModifier.Value = 2;
            world.MaxFoodItemsPerTick.Value = 120;
            world.MaxEnergyPerFoodItem.Value = 140;
            world.MaxFoodItems.Value = 1000;
            world.BirthEnergyShare.Value = 30;
            world.PoisonEffectiveness.Value = 2;
            world.PoisonResistEnergyDrain.Value = 5;

            var initPopulation = new List<Individual>(InitPopulationSize);
            for (int i = 0; i < InitPopulationSize; i++)
            {
                var individual = RandomIndividuals 
                    ? world.Mutator.GenerateRandom() 
                    : world.Mutator.GenerateAverage();

                initPopulation.Add(individual);
            }
            world.SpreadIndividuals(initPopulation, new Coord(0, 0), Size);
            world.SpreadFood();
            return world;
        }
    }
}
