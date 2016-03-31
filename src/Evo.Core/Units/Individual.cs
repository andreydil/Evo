﻿using System;
using System.Diagnostics;
using System.Linq;
using Evo.Core.Basic;
using Evo.Core.Universe;

namespace Evo.Core.Units
{
    public class Individual : Unit
    {
        private readonly World _world;
        public readonly Gene[] Genome;

        public Individual(ulong id, World world) : base(id)
        {
            _world = world;
            Genome = new[] { Color, Aggression, Strength, Fertility, LifeTime, Purpose, SightRange, MinEnergyAcceptable };
            Energy.Value = Energy.Max / 2;
            Age.Value = 0;
            Desire.Value = 1;
        }

        #region Genome

        public Gene Color { get; set; } = new Gene(0x000000, 0xffffff);
        public Gene Aggression { get; set; } = new Gene(1, 1000);
        public Gene Strength { get; set; } = new Gene(1, 1000);
        public Gene Fertility { get; set; } = new Gene(1, 1000);
        public Gene LifeTime { get; set; } = new Gene(100, 10000);
        public Gene Purpose { get; set; } = new Gene(1, Constants.Probability100Percent);
        public Gene SightRange { get; set; } = new Gene(2, 30);
        public Gene MinEnergyAcceptable { get; set; } = new Gene(10, 1000);

        #endregion

        #region Current State

        public LimitedInt Energy { get; set; } = new LimitedInt(0, 1000);
        public LimitedInt Age { get; set; } = new LimitedInt(0, 10000);
        public LimitedInt Desire { get; set; } = new LimitedInt(0, 1000);
        public Target Target { get; set; } = new Target();

        #endregion

        public bool IsAlive => Energy > 0 && Age <= LifeTime;

        public int EnergyDrainPerTick
            => (int)Math.Round((Strength.NormalizedValue + Aggression.NormalizedValue) * _world.EnergyDrainModificator, MidpointRounding.AwayFromZero) + 1;

        public int MaxEnergy => 1000;

        public double GetNormalizedDifference(Individual other)
        {
            var difference = GetDifference(other);
            var maxDifference = GetMaxDifference();
            return difference / maxDifference;
        }

        private double GetDifference(Individual other)
        {
            return Math.Sqrt((Color - (double)other.Color) * (Color - other.Color));
        }

        private double GetMaxDifference()
        {
            var minColor = new Gene(0, 0xffffff) { Value = 0 };
            var maxColor = new Gene(0, 0xffffff) { Value = 0xffffff };
            return Math.Sqrt((minColor - (double)maxColor) * (minColor - maxColor));
        }

        public void LiveOneTick()
        {
            ChangeTarget();
            Step();
            ++Age.Value;
            Energy.Value -= EnergyDrainPerTick;
            Desire.Value += Fertility / 40 + 1;
        }

        private void ChangeTarget()
        {
            var food = _world.Navigator.FindClosestFood(Point, SightRange);

            if (food != null && Energy < MinEnergyAcceptable) //too hungry
            {
                Target.TargetType = TargetType.Eat;
                Target.Id = food.Id;
                Target.Direction = Point.FindDirection(food.Point);
                return;
            }

            var individual = _world.Navigator.FindClosestIndividual(Point, 20);
            double distanceToIndividual, strengthQuotient, differenceFromIndividual;
            if (individual != null)
            {
                distanceToIndividual = Point.GetDistanceTo(individual.Point);
                strengthQuotient = (double)Strength / individual.Strength;
                differenceFromIndividual = GetNormalizedDifference(individual);
            }
            else
            {
                distanceToIndividual = double.MaxValue;
                strengthQuotient = 0;
                differenceFromIndividual = 0;
            }

            if (!(TargetStillValid() && _world.CheckRng(Purpose)))
            {
                var distanceToFood = food == null ? double.MaxValue : Point.GetDistanceTo(food.Point);
                var wantEat = _world.DecisionMaker.WantEat(Energy, MaxEnergy, distanceToFood);
                var wantSex = _world.DecisionMaker.WantSex(Desire.NormalizedValue, distanceToIndividual, differenceFromIndividual);
                var wantKill = _world.DecisionMaker.WantKill(Aggression.NormalizedValue, distanceToIndividual, differenceFromIndividual, strengthQuotient);

                Target.TargetType = _world.DecisionMaker.DecideTarget(wantEat, wantSex, wantKill);
                switch (Target.TargetType)
                {
                    case TargetType.Eat:
                        _world.IncStats.AddStat("Decisions to Eat");
                        break;
                    case TargetType.Walk:
                        _world.IncStats.AddStat("Decisions to Walk");
                        break;
                    case TargetType.Sex:
                        _world.IncStats.AddStat("Decisions to have Sex");
                        break;
                    case TargetType.Kill:
                        _world.IncStats.AddStat("Decisions to Kill");
                        break;
                }
            }

            switch (Target.TargetType)
            {
                case TargetType.Eat:
                    if (food != null)
                    {
                        Target.Direction = Point.FindDirection(food.Point);
                    }
                    break;
                case TargetType.Walk:
                    if (_world.CheckRng(Purpose))
                    {
                        Target.Direction.X = _world.Random.Next(-1, 1);
                        Target.Direction.Y = _world.Random.Next(-1, 1);
                    }
                    break;
                case TargetType.Sex:
                case TargetType.Kill:
                    if (individual != null)
                    {
                        Target.Id = individual.Id;
                        Target.Direction = Point.FindDirection(individual.Point);
                    }
                    break;
            }
        }

        private bool TargetStillValid()
        {
            switch (Target.TargetType)
            {
                case TargetType.Walk:
                    return true;
                case TargetType.Eat:
                    var foodItem = _world.Food.SingleOrDefault(f => f.Id == Target.Id);
                    return foodItem != null;
                case TargetType.Sex:
                case TargetType.Kill:
                    var individual = _world.Population.SingleOrDefault(f => f.Id == Target.Id);
                    return individual != null;
                default:
                    return false;
            }
        }

        private void Step()
        {
            _world.Navigator.BounceFromMapBorders(Point, Target.Direction);
            var pointToStep = Point + Target.Direction;
            if (Point == pointToStep)
            {
                return;
            }

            var partner = _world.Navigator.FindIndividual(pointToStep);

            switch (Target.TargetType)
            {
                case TargetType.Walk:
                case TargetType.Eat:
                    if (partner == null)
                    {
                        var food = _world.Navigator.FindFood(pointToStep);
                        if (food != null) //eat
                        {
                            Energy.Value += food.Energy;
                            _world.Food.Remove(food);
                            Target.TargetType = TargetType.Walk;
                        }
                        if (_world.Navigator.FindIndividual(pointToStep) == null)
                        {
                            Point = pointToStep;
                        }
                    }
                    break;
                case TargetType.Sex:
                    if (partner != null && partner.Id == Target.Id)
                    {
                        if (_world.CheckRng(Fertility + partner.Fertility, 0, Fertility.Max * 2))
                        {
                            var child = _world.Mutator.GenerateChild(this, partner);
                            child.Point = _world.Navigator.PlaceChild(Point, partner.Point);
                            Energy.Value -= Energy / 4;
                            partner.Energy.Value -= partner.Energy / 4;
                            child.Energy.Value = Energy / 4 + partner.Energy / 4;
                            if (child.Point != null)
                            {
                                _world.AddIndividual(child);
                                _world.IncStats.AddStat("Births");
                            }
                        }
                        Desire.Value = 0;
                        partner.Desire.Value = 0;
                        Target.TargetType = TargetType.Walk;
                    }
                    else
                    {
                        Point = pointToStep;
                    }
                    break;
                case TargetType.Kill:
                    if (partner != null && partner.Id == Target.Id)
                    {
                        if (_world.CheckRng(Strength, 0, Strength + partner.Strength))
                        {
                            Kill(this, partner);
                            Target.TargetType = TargetType.Walk;
                        }
                        else
                        {
                            Kill(partner, this);
                        }
                    }
                    else
                    {
                        Point = pointToStep;
                    }
                    break;
            }
        }

        private void Kill(Individual killer, Individual victim)
        {
            _world.Population.Remove(victim);
            var foodItem = new FoodItem(_world.GenerateId())
            {
                Energy = victim.Energy / 2,
                Point = victim.Point,
            };
            _world.AddFood(foodItem);
            killer.Energy.Value -= (int)(2 * EnergyDrainPerTick * ((double)victim.Strength / Strength));
            killer.Target.TargetType = TargetType.Eat;
            killer.Target.Id = foodItem.Id;

            _world.IncStats.AddStat("Kills");
        }
    }
}