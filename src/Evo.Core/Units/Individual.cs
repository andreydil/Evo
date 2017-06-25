using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Evo.Core.Basic;
using Evo.Core.Universe;

namespace Evo.Core.Units
{
    [Serializable]
    public class Individual : Unit
    {
        private readonly World _world;
        public readonly Dictionary<string, Gene> Genome;
        public readonly int MinGeneration;
        public readonly int MaxGeneration;

        public Individual(ulong id, World world, int minGeneration = 1, int maxGeneration = 1) : base(id)
        {
            _world = world;
            MinGeneration = minGeneration;
            MaxGeneration = maxGeneration;
            Genome = new Dictionary<string, Gene>
            {
                { GeneNames.Color, Color },
                { GeneNames.Aggression, Aggression },
                { GeneNames.Strength, Strength },
                { GeneNames.Fertility, Fertility },
                { GeneNames.LifeTime, LifeTime },
                { GeneNames.Purpose, Purpose },
                { GeneNames.SightRange, SightRange },
                { GeneNames.MinEnergyAcceptable, MinEnergyAcceptable },
            };

            Energy.Value = Energy.Max / 2;
            Age.Value = 0;
            Desire.Value = 1;
        }

        public override UnitType Type => UnitType.Individual;

        #region Genome

        public ColorGene Color { get; set; } = new ColorGene();
        public Gene Aggression { get; set; } = new Gene(1, 1000);
        public Gene Strength { get; set; } = new Gene(1, 1000);
        public Gene Fertility { get; set; } = new Gene(1, 1000);
        public Gene LifeTime { get; set; } = new Gene(100, 10000);
        public Gene Purpose { get; set; } = new Gene(1, Constants.Probability100Percent);
        public Gene SightRange { get; set; } = new Gene(2, 30);
        public Gene MinEnergyAcceptable { get; set; } = new Gene(10, 800);

        #endregion

        #region Current State

        public LimitedInt Energy { get; set; } = new LimitedInt(0, 1000);
        public LimitedInt Age { get; set; } = new LimitedInt(0, 10000);
        public LimitedInt Desire { get; set; } = new LimitedInt(0, 1000);
        public Target Target { get; set; } = new Target();

        #endregion

        public bool IsKilled { get; set; }
        public bool IsAlive => !IsKilled && Energy > 0 && Age <= LifeTime;

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
            return Color.GetDifferenceFrom(other.Color);
        }

        private double GetMaxDifference()
        {
            var minColor = new ColorGene { Value = 0 };
            var maxColor = new ColorGene { Value = 0xffffff };
            return maxColor.GetDifferenceFrom(minColor);
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

            if (Energy < MinEnergyAcceptable) //too hungry
            {
                if (food != null)
                {
                    Target.TargetType = TargetType.Eat;
                    Target.Id = food.Id;
                    Target.Direction = Point.FindDirection(food.Point);
                    return;
                }
                Target.TargetType = TargetType.Walk;
                if (_world.CheckRng(Purpose))
                {
                    Target.Direction = GetRandomDirection();
                }
            }

            var individual = _world.Navigator.FindClosestIndividual(Point, SightRange);
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
                        _world.AdditionalStats.AddStat("Decisions to Eat");
                        break;
                    case TargetType.Walk:
                        _world.AdditionalStats.AddStat("Decisions to Walk");
                        break;
                    case TargetType.Sex:
                        _world.AdditionalStats.AddStat("Decisions to have Sex");
                        break;
                    case TargetType.Kill:
                        _world.AdditionalStats.AddStat("Decisions to Kill");
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
                    if (!_world.CheckRng(Purpose))
                    {
                        Target.Direction = GetRandomDirection();
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

        private Coord GetRandomDirection()
        {
            return new Coord(_world.Random.Next(-1, 2), _world.Random.Next(-1, 2));
        }

        private bool TargetStillValid()
        {
            switch (Target.TargetType)
            {
                case TargetType.Walk:
                    return true;
                case TargetType.Eat:
                    var foodItem = _world.FindFoodById(Target.Id);
                    return foodItem != null;
                case TargetType.Sex:
                case TargetType.Kill:
                    var individual = _world.FindIndividualById(Target.Id);
                    return individual != null;
                default:
                    return false;
            }
        }

        private void Step()
        {
            Target.Direction = _world.Navigator.BounceFromWalls(Point, Target.Direction);
            var pointToStep = Point + Target.Direction;
            if (Point == pointToStep)
            {
                return;
            }

            var partner = _world.Navigator.FindIndividual(pointToStep);
            var food = _world.Navigator.FindFood(pointToStep);

            switch (Target.TargetType)
            {
                case TargetType.Walk:
                case TargetType.Eat:
                    if (partner == null)
                    {
                        Eat(food);
                        _world.Navigator.MoveUnit(this, pointToStep);
                    }
                    break;
                case TargetType.Sex:
                    if (partner != null)
                    {
                        if (partner.Id == Target.Id)
                        {
                            if (_world.CheckRng(Fertility + partner.Fertility, 0, Fertility.Max * 2))
                            {
                                var child = _world.Mutator.GenerateChild(this, partner);
                                var newPoint = _world.Navigator.PlaceChild(Point, partner.Point);
                                Energy.Value -= (int)(Energy * _world.BirthEnergyShare.NormalizedValue);
                                partner.Energy.Value -= (int)(partner.Energy * _world.BirthEnergyShare.NormalizedValue);
                                child.Energy.Value = (int)((Energy + partner.Energy) * _world.BirthEnergyShare.NormalizedValue);
                                if (newPoint.HasValue)
                                {
                                    child.Point = newPoint.Value;
                                    _world.AddIndividual(child);
                                    _world.MainStats.AddStat("Births");
                                }
                            }
                            Desire.Value = 0;
                            partner.Desire.Value = 0;
                            Target.TargetType = TargetType.Walk;
                        }
                    }
                    else
                    {
                        Eat(food);
                        _world.Navigator.MoveUnit(this, pointToStep);
                    }
                    break;
                case TargetType.Kill:
                    if (partner != null)
                    {
                        if (partner.Id == Target.Id)
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
                    }
                    else
                    {
                        Eat(food);
                        _world.Navigator.MoveUnit(this, pointToStep);
                    }
                    break;
            }
        }

        private void Eat(FoodItem food)
        {
            if (food != null)
            {
                Energy.Value += food.Energy;
                _world.RemoveFood(food);
                Target.TargetType = TargetType.Walk;
            }
        }

        public void Kill(Individual killer, Individual victim)
        {
            victim.IsKilled = true;
            _world.RemoveIndividual(victim);
            var foodItem = new FoodItem(_world.GenerateId())
            {
                Energy = victim.Energy / 2,
                Point = victim.Point,
            };
            _world.AddFood(foodItem);
            killer.Energy.Value -= (int)(2 * EnergyDrainPerTick * ((double)victim.Strength / Strength));
            killer.Target.TargetType = TargetType.Eat;
            killer.Target.Id = foodItem.Id;

            _world.MainStats.AddStat("Kills");
        }
    }
}
