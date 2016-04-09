using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
