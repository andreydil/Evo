﻿using System;
using System.Linq;
using System.Runtime.Serialization;
using Evo.Core.Basic;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    [Serializable]
    public class Mutator
    {
        private readonly World _world;

        public Mutator(World world)
        {
            _world = world;
        }

        public Individual GenerateRandom()
        {
            var individual = new Individual(_world.GenerateId(), _world);
            foreach (var geneItem in individual.Genome)
            {
                geneItem.Value.Value = _world.Random.Next(geneItem.Value.Min, geneItem.Value.Max);
            }
            return individual;
        }

        public Individual GenerateAverage()
        {
            return new Individual(_world.GenerateId(), _world);
        }

        public Individual GenerateChild(Individual father, Individual mother)
        {
            var individual = new Individual(_world.GenerateId(), _world, Math.Min(father.MinGeneration, mother.MinGeneration) + 1, Math.Max(father.MaxGeneration, mother.MaxGeneration) + 1);
            foreach (var geneItem in individual.Genome)
            {
                var gene = geneItem.Value;
                gene.Value = _world.Random.Next(Math.Min(father.Genome[geneItem.Key], mother.Genome[geneItem.Key]), Math.Max(father.Genome[geneItem.Key], mother.Genome[geneItem.Key]) + 1); //somewhere between father and mother
                if (_world.CheckRng(_world.MutationProbability))
                {
                    gene.Value += _world.Random.Next(-_world.MutationMaxDelta, _world.MutationMaxDelta + 1);
                }
            }

            individual.Color.SetRGB(GenerateColorComponent(father.Color.Red, mother.Color.Red),
                                    GenerateColorComponent(father.Color.Green, mother.Color.Green),
                                    GenerateColorComponent(father.Color.Blue, mother.Color.Blue));

            return individual;
        }

        public Individual GenerateIndividual(Func<Gene, int> geneFunc, ulong id, World world)
        {
            var individual = new Individual(id, world);
            foreach (var gene in individual.Genome.Select(gi => gi.Value))
            {
                gene.Value = geneFunc(gene);
            }
            return individual;
        }

        private byte GenerateColorComponent(byte fatherComponent, byte motherComponent)
        {
            int component = _world.Random.Next(Math.Min(fatherComponent, motherComponent), Math.Max(fatherComponent, motherComponent) + 1);
            int mutationMaxDelta = 0xff * _world.MutationMaxDelta / _world.MutationMaxDelta.Max;
            if (_world.CheckRng(_world.MutationProbability))
            {
                component += _world.Random.Next(-mutationMaxDelta, mutationMaxDelta + 1);
            }
            if (component < 0)
            {
                return 0;
            }
            if (component > 0xff)
            {
                return 0xff;
            }
            return (byte)component;
        }
    }
}
