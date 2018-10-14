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
            world.MutationProbability.Value = 100;
            world.MutationMaxDelta.Value = 100;
            world.EnergyDrainModifier.Value = 2;
            world.MaxFoodItemsPerTick.Value = 150;
            world.MaxEnergyPerFoodItem.Value = 150;
            world.MaxFoodItems.Value = 15000;
            world.BirthEnergyShare.Value = 30;
            world.PoisonEffectiveness.Value = 2;
            world.PoisonResistEnergyDrain.Value = 5;
            world.AttackerDamageModifier.Value = 1;

            const int initPopulationCount = 100;
            var initPopulation = new List<Individual>(initPopulationCount);
            for (int i = 0; i < initPopulationCount; i++)
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
