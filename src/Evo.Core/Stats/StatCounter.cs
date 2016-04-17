using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evo.Core.Basic;
using Evo.Core.Units;

namespace Evo.Core.Stats
{
    public class StatCounter
    {
        public Individual GetAverage(ICollection<Individual> individuals)
        {
            long colorR = 0;
            long colorG = 0;
            long colorB = 0;
            long aggression  = 0;
            long strength  = 0;
            long fertility  = 0;
            long lifeTime  = 0;
            long purpose = 0;
            long sightRange = 0;
            long minEnergyAcceptable = 0;

            long energy = 0;
            long age = 0;
            long desire = 0;

            foreach (var individual in individuals)
            {
                colorR += individual.Color.Red;
                colorG += individual.Color.Green;
                colorB += individual.Color.Blue;
                aggression += individual.Aggression;
                strength += individual.Strength;
                fertility += individual.Fertility;
                lifeTime += individual.LifeTime;
                purpose += individual.Purpose;
                sightRange += individual.SightRange;
                minEnergyAcceptable += individual.MinEnergyAcceptable;

                energy += individual.Energy;
                age += individual.Age;
                desire += individual.Desire;
            }

            var count = individuals.Count;

            var average = new Individual(0, null);

            if (count == 0)
            {
                return average;
            }
            average.Color.Value = (int)(colorR / count * 0x10000 + colorG / count * 0x100 + colorB / count);
            average.Aggression.Value = (int)(aggression / count);
            average.Strength.Value = (int)(strength / count);
            average.Fertility.Value = (int)(fertility / count);
            average.LifeTime.Value = (int)(lifeTime / count);
            average.Purpose.Value = (int)(purpose / count);
            average.SightRange.Value = (int)(sightRange / count);
            average.MinEnergyAcceptable.Value = (int)(minEnergyAcceptable / count);
            average.Energy.Value = (int)(energy / count);
            average.Age.Value = (int)(age / count);
            average.Desire.Value = (int)(desire / count);

            return average;
        }

        public int GetEuclideanDistance(Individual individual1, Individual individual2)
        {
            long sum = 0;
            foreach (var geneItem in individual1.Genome)
            {
                var gene1 = geneItem.Value;
                var gene2 = individual2.Genome[geneItem.Key];
                if (geneItem.Key == GeneNames.Color)
                {
                    var color1 = (ColorGene)gene1;
                    var color2 = (ColorGene)gene2;
                    sum += (color1.Red - color2.Red) * (color1.Red - color2.Red);
                    sum += (color1.Green - color2.Green) * (color1.Green - color2.Green);
                    sum += (color1.Blue - color2.Blue) * (color1.Blue - color2.Blue);
                }
                else
                {
                    sum += (gene1 - gene2) * (gene1 - gene2);
                }
            }
            return (int)Math.Round(Math.Sqrt(sum));
        }

        public int GetDifference(Individual individual1, Individual individual2)
        {
            int sum = 0;
            foreach (var geneItem in individual1.Genome)
            {
                var gene1 = geneItem.Value;
                var gene2 = individual2.Genome[geneItem.Key];
                if (geneItem.Key == GeneNames.Color)
                {
                    var color1 = (ColorGene)gene1;
                    var color2 = (ColorGene)gene2;
                    sum += Math.Abs(color1.Red - color2.Red);
                    sum += Math.Abs(color1.Green - color2.Green);
                    sum += Math.Abs(color1.Blue - color2.Blue);
                }
                else
                {
                    sum += Math.Abs(gene1 - gene2);
                }
            }
            return sum;
        }
    }
}
