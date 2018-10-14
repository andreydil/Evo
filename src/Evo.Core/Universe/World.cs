using System;
using System.Collections.Generic;
using System.Linq;
using Evo.Core.Basic;
using Evo.Core.Stats;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    [Serializable]
    public class World
    {
        public readonly Random Random;
        public readonly Mutator Mutator;
        public readonly Navigator Navigator;
        public readonly DecisionMaker DecisionMaker;
        public readonly Coord Size;
        public readonly Dictionary<string, LimitedInt> Tuners;
        public readonly StatCounter StatCounter;
        private readonly List<Individual> _population = new List<Individual>();
        private readonly List<FoodItem> _food = new List<FoodItem>();
        private ulong _idGenerator = 0;

        public World(Random random, Coord size)
        {
            Random = random;
            Size = size;
            Mutator = new Mutator(this);
            Navigator = new Navigator(this);
            DecisionMaker = new DecisionMaker(this);
            StatCounter = new StatCounter();
            Tuners = new Dictionary<string, LimitedInt>
            {
                { nameof(MaxFoodItems), MaxFoodItems },
                { nameof(MaxFoodItemsPerTick), MaxFoodItemsPerTick },
                { nameof(MaxEnergyPerFoodItem), MaxEnergyPerFoodItem },
                { nameof(MutationProbability), MutationProbability },
                { nameof(MutationMaxDelta), MutationMaxDelta },
                { nameof(EnergyDrainModifier), EnergyDrainModifier },
                { nameof(BirthEnergyShare), BirthEnergyShare },
                { nameof(EatDecisionModifier), EatDecisionModifier },
                { nameof(SexDecisionModifier), SexDecisionModifier },
                { nameof(KillDecisionModifier), KillDecisionModifier },
                { nameof(PoisonEffectiveness), PoisonEffectiveness },
                { nameof(PoisonResistEnergyDrain), PoisonResistEnergyDrain },
                { nameof(AttackerDamageModifier), AttackerDamageModifier },
            };
        }

        //tuners
        public LimitedInt MutationProbability { get; set; } = new LimitedInt(1, Constants.Probability100Percent);
        public LimitedInt MutationMaxDelta { get; set; } = new LimitedInt(1, 1000);
        public LimitedInt MaxFoodItemsPerTick { get; set; } = new LimitedInt(1, 1000);
        public LimitedInt MaxEnergyPerFoodItem { get; set; } = new LimitedInt(1, 1000);
        public LimitedInt MaxFoodItems { get; set; } = new LimitedInt(1, 50000);
        public LimitedInt EnergyDrainModifier { get; set; } = new LimitedInt(1, 20);
        public LimitedInt BirthEnergyShare { get; set; } = new LimitedInt(1, 100);
        public LimitedInt EatDecisionModifier { get; set; } = new LimitedInt(1, 10);
        public LimitedInt SexDecisionModifier { get; set; } = new LimitedInt(1, 10);
        public LimitedInt KillDecisionModifier { get; set; } = new LimitedInt(1, 10);
        public LimitedInt PoisonEffectiveness { get; set; } = new LimitedInt(1, 10);
        public LimitedInt PoisonResistEnergyDrain { get; set; } = new LimitedInt(1, 10);
        public LimitedInt AttackerDamageModifier { get; set; } = new LimitedInt(1, 100);

        public Individual AverageIndividual => StatCounter.GetAverage(_population);

        public ulong Tick { get; private set; } = 0;
        public IEnumerable<Individual> Population => _population.AsEnumerable();
        public IEnumerable<FoodItem> Food => _food.AsEnumerable();
        public IEnumerable<PoisonArea> PoisonAreas => _poisonAreas;
        private readonly List<PoisonArea> _poisonAreas = new List<PoisonArea>();

        public readonly IncStats MainStats = new IncStats();
        public readonly IncStats AdditionalStats = new IncStats();
        public readonly TimeSpanStats<int> PopulationSize = new TimeSpanStats<int>(1000);
        public readonly TimeSpanStats<int> FoodAmount = new TimeSpanStats<int>(1000);

        public ulong GenerateId()
        {
            return _idGenerator++;
        }

        public void Live1Tick()
        {
            if (_population.Count == 0)
            {
                return;
            }
            SpreadFood();
            foreach (var individual in _population.ToList())
            {
                if (individual.IsAlive)
                {
                    individual.LiveOneTick(GetExtInfluenceForCoord(individual.Point));
                }
                else
                {
                    RemoveIndividual(individual);
                    if (individual.Energy <= individual.Energy.Min)
                    {
                        MainStats.AddStat("Deaths from hunger");
                    }
                    else if (individual.Age >= individual.Age.Max)
                    {
                        MainStats.AddStat("Deaths from age");
                    }
                    else
                    {
                        MainStats.AddStat("Other deaths");
                    }
                }
            }
            PopulationSize.Add(Tick, _population.Count);
            FoodAmount.Add(Tick, _food.Count);
            ++Tick;
        }

        private ExtInfluence GetExtInfluenceForCoord(Coord point)
        {
            return new ExtInfluence
            {
                Poison = GetPoisonIntensityAtPoint(point),
            };
        }

        public void AddIndividual(Individual individual)
        {
            Navigator.PutUnit(individual);
            _population.Add(individual);
        }

        public void AddIndividuals(IEnumerable<Individual> individuals)
        {
            foreach (var individual in individuals)
            {
                AddIndividual(individual);
            }
        }

        public void RemoveIndividual(Individual individual)
        {
            Navigator.RemoveUnit(individual);
            _population.Remove(individual);
        }

        public void AddFood(FoodItem foodItem)
        {
            Navigator.PutUnit(foodItem);
            _food.Add(foodItem);
        }

        public void RemoveFood(FoodItem foodItem)
        {
            Navigator.RemoveUnit(foodItem);
            _food.Remove(foodItem);
        }

        public void AddPoisonArea(Coord point1, Coord point2, int intensity)
        {
            var topLeft = new Coord(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            var bottomRight = new Coord(Math.Max(point1.X, point2.X), Math.Max(point1.Y, point2.Y));
            _poisonAreas.Add(new PoisonArea
            {
                TopLeft = topLeft,
                BottomRight = bottomRight,
                Intensity = intensity,
            });
        }

        public void RemovePoisonAreas(Coord point)
        {
            for (int i = 0; i < _poisonAreas.Count;)
            {
                var curArea = _poisonAreas[i];
                if (curArea.TopLeft.X <= point.X && curArea.BottomRight.X >= point.X
                    && curArea.TopLeft.Y <= point.Y && curArea.BottomRight.Y >= point.Y)
                {
                    _poisonAreas.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
        }

        public int GetPoisonIntensityAtPoint(Coord point)
        {
            for (int i = _poisonAreas.Count - 1; i >= 0; --i)
            {
                var curArea = _poisonAreas[i];
                if (curArea.TopLeft.X <= point.X && curArea.TopLeft.Y <= point.Y && curArea.BottomRight.X >= point.X && curArea.BottomRight.Y >= point.Y)
                {
                    return curArea.Intensity;
                }
            }
            return 0;
        }

        public bool CheckRng(LimitedInt range)
        {
            return CheckRng(range.Value, range.Min, range.Max);
        }

        public bool CheckRng(int value, int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue + 1) <= value;
        }

        public void SpreadFood()
        {
            if (_food.Count >= MaxFoodItems)
            {
                return;
            }
            var foodCount = Random.Next(Math.Min(MaxFoodItemsPerTick, MaxFoodItems - _food.Count));
            SpreadFood(foodCount, new Coord(0, 0), new Coord(Size.X, Size.Y));
        }

        public void SpreadFood(int foodCount, Coord topLeft, Coord bottomRight)
        {
            topLeft = Navigator.EnsureBounds(topLeft);
            bottomRight = Navigator.EnsureBounds(bottomRight);
            const int tryCount = 100;
            for (int i = 0; i < foodCount; i++)
            {
                var newFoodItem = new FoodItem(GenerateId())
                {
                    Energy = Random.Next(1, MaxEnergyPerFoodItem + 1),
                };
                for (int j = 0; j < tryCount; j++)
                {
                    newFoodItem.Point = new Coord(topLeft.X + Random.Next(bottomRight.X - topLeft.X), topLeft.Y + Random.Next(bottomRight.Y - topLeft.Y));

                    if (Navigator.IsWall(newFoodItem.Point))
                    {
                        continue;
                    }

                    var unitInPoint = Navigator.FindUnit(newFoodItem.Point);
                    if (unitInPoint == null)
                    {
                        AddFood(newFoodItem);
                        break;
                    }

                    var individual = unitInPoint as Individual;
                    if (individual != null)
                    {
                        continue;
                    }

                    var food = unitInPoint as FoodItem;
                    if (food != null)
                    {
                        food.Energy += newFoodItem.Energy;
                        break;
                    }
                }
            }
        }

        public void SpreadIndividuals(IEnumerable<Individual> individuals, Coord topLeftPoint, Coord bottomRightPoint)
        {
            foreach (var individual in individuals)
            {
                for (int i = 0; i < 100; i++)
                {
                    individual.Point = Navigator.EnsureBounds(new Coord(Random.Next(topLeftPoint.X, bottomRightPoint.X), Random.Next(topLeftPoint.Y, bottomRightPoint.Y)));
                    if (Navigator.FindIndividual(individual.Point) != null)
                    {
                        continue;
                    }

                    AddIndividual(individual);
                    break;
                }
            }
        }


        public void KillUnitAtPoint(int x, int y)
        {
            var unit = Navigator.FindUnit(x, y);
            var individual = unit as Individual;
            if (individual != null)
            {
                RemoveIndividual(individual);
            }
            else
            {
                var foodItem = unit as FoodItem;
                if (foodItem != null)
                {
                    RemoveFood(foodItem);
                }
            }
        }

        public Individual FindIndividualById(ulong id)
        {
            var individual = new Individual(id, null);
            return FindBUnitById(_population, individual);
        }

        public FoodItem FindFoodById(ulong id)
        {
            var food = new FoodItem(id);
            return FindBUnitById(_food, food);
        }

        private static T FindBUnitById<T>(List<T> list, T unit) where T : Unit
        {
            var i = list.BinarySearch(unit, new UnitIdComparer());
            return i < 0 ? null : list[i];
        }
    }
}
