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
            return new Individual(_world.GenerateId(), _world);
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

            individual.Color.SetRGB(GenerateColorComponent(father.Color.Red, mother.Color.Red),
                                    GenerateColorComponent(father.Color.Green, mother.Color.Green),
                                    GenerateColorComponent(father.Color.Blue, mother.Color.Blue));

            return individual;
        }

        private byte GenerateColorComponent(byte fatherComponent, byte motherComponent)
        {
            int component = _world.Random.Next(Math.Min(fatherComponent, motherComponent), Math.Max(fatherComponent, motherComponent));
            int mutationMaxDelta = 0xff * _world.MutationMaxDelta / _world.MutationMaxDelta.Max;
            if (_world.CheckRng(_world.MutationProbability))
            {
                component += _world.Random.Next(-mutationMaxDelta, mutationMaxDelta);
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
