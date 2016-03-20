using System;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
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
            foreach (var gene in individual.Genome)
            {
                gene.Value = _world.Random.Next(gene.Min, gene.Max);
            }
            return individual;
        }

        public Individual GenerateAverage()
        {
            var individual = new Individual(_world.GenerateId(), _world);
            foreach (var gene in individual.Genome)
            {
                gene.Value = (gene.Min + gene.Max) / 2;
            }
            return individual;
        }

        public Individual GenerateChild(Individual father, Individual mother)
        {
            var individual = new Individual(_world.GenerateId(), _world);
            for (int i = 0; i < individual.Genome.Length; i++)
            {
                var gene = individual.Genome[i];
                gene.Value = _world.Random.Next(Math.Min(father.Genome[i], mother.Genome[i]), Math.Max(father.Genome[i], mother.Genome[i])); //somewhere between father and mother
                if (_world.CheckRng(_world.MutationProbability))
                {
                    gene.Value += _world.Random.Next(-_world.MutationMaxDelta, _world.MutationMaxDelta);
                }
            }
            return individual;
        }
    }
}
