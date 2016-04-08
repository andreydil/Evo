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

            var average = new Individual(0, null)
            {
                Color = {Value = (int)(colorR/count*0x10000 + colorG/count*0x100+colorB/count) },
                Aggression = { Value = (int)(aggression / count) },
                Strength = { Value = (int)(strength / count) },
                Fertility = { Value = (int)(fertility / count) },
                LifeTime = { Value = (int)(lifeTime / count) },
                Purpose = { Value = (int)(purpose / count) },
                SightRange = { Value = (int)(sightRange / count) },
                MinEnergyAcceptable = { Value = (int)(minEnergyAcceptable / count) },

                Energy = { Value = (int)(energy / count) },
                Age = { Value = (int)(age / count) },
                Desire = { Value = (int)(desire / count) }
            };
            
            return average;
        }
    }
}
