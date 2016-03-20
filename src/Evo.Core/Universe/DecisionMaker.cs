using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    public class DecisionMaker
    {
        private readonly World _world;

        public DecisionMaker(World world)
        {
            _world = world;
        }

        //TODO: add world's modificators to formulas?
        public double WantEat(int curEnergy, int maxEnergy, double distance)
        {
            return (maxEnergy - curEnergy) / (double)maxEnergy / distance;
        }

        public double WantSex(double normalizedDesire, double distance, double difference)
        {
            if (difference <= 0.000001)
            {
                difference = 0.001;
            }
            return normalizedDesire / distance * (1.0 - difference);
        }

        public double WantKill(double normalizedAggression, double distance, double difference, double strengthQuotient)
        {
            if (difference <= 0.000001)
            {
                difference = 0.001;
            }
            return normalizedAggression / distance * difference * strengthQuotient;
        }

        public TargetType DecideTarget(double wantEat, double wantSex, double wantKill)
        {
            double wantWalk = wantEat * 2;

            var r = _world.Random.Next((int)(1000 * (wantEat + wantSex + wantKill + wantWalk)));
            Debug.WriteLine("wantEat={0}; wantWalk = {1}; wantSex={2}; wantKill={3}; sum={4}; ",
                wantEat.ToString("0.00"), wantWalk.ToString("0.00"), wantSex.ToString("0.00"), wantKill.ToString("0.00"), (wantEat + wantWalk + wantSex + wantKill).ToString("0.00"));
            if (r <= wantEat * 1000)
            {
                return TargetType.Eat;
            }
            if (r <= (wantEat + wantSex) * 1000)
            {
                return TargetType.Sex;
            }
            if (r <= (wantEat + wantSex + wantKill) * 1000)
            {
                return TargetType.Kill;
            }
            return TargetType.Walk;
        }
    }
}
