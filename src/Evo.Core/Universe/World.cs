using System;
using System.Collections.Generic;
using System.Linq;
using Evo.Core.Basic;
using Evo.Core.Stats;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    public class World
    {
        public readonly Random Random;
        public readonly Mutator Mutator;
        public readonly Navigator Navigator;
        public readonly DecisionMaker DecisionMaker;
        public readonly Coord Size;
        public readonly Dictionary<string, LimitedInt> Tuners;
        private readonly StatCounter _statCounter;
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
            _statCounter = new StatCounter();
            Tuners = new Dictionary<string, LimitedInt>
            {
                { nameof(MaxFoodItems), MaxFoodItems },
                { nameof(MaxFoodItemsPerTick), MaxFoodItemsPerTick },
                { nameof(MaxEneryPerFoodItem), MaxEneryPerFoodItem },
                { nameof(MutationProbability), MutationProbability },
                { nameof(MutationMaxDelta), MutationMaxDelta },
                { nameof(EnergyDrainModificator), EnergyDrainModificator },
                { nameof(BirthEnergyShare), BirthEnergyShare },
                { nameof(EatDecisionModificator), EatDecisionModificator },
                { nameof(SexDecisionModificator), SexDecisionModificator },
                { nameof(KillDecisionModificator), KillDecisionModificator },
            };
        }

        //tuners
        public LimitedInt MutationProbability { get; set; } = new LimitedInt(1, Constants.Probability100Percent);
        public LimitedInt MutationMaxDelta { get; set; } = new LimitedInt(1, 1000);
        public LimitedInt MaxFoodItemsPerTick { get; set; } = new LimitedInt(1, 1000);
        public LimitedInt MaxEneryPerFoodItem { get; set; } = new LimitedInt(1, 1000);
        public LimitedInt MaxFoodItems { get; set; } = new LimitedInt(1, 10000);
        public LimitedInt EnergyDrainModificator { get; set; } = new LimitedInt(1, 100);
        public LimitedInt BirthEnergyShare { get; set; } = new LimitedInt(1, 100);
        public LimitedInt EatDecisionModificator { get; set; } = new LimitedInt(1, 10);
        public LimitedInt SexDecisionModificator { get; set; } = new LimitedInt(1, 10);
        public LimitedInt KillDecisionModificator { get; set; } = new LimitedInt(1, 10);

        public Individual AverageIndividual => _statCounter.GetAverage(_population);

        public ulong Tick { get; private set; } = 0;
        public IEnumerable<Individual> Population => _population.AsEnumerable();
        public IEnumerable<FoodItem> Food => _food.AsEnumerable();

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
                    individual.LiveOneTick();
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

            const int tryCount = 100;
            var foodCount = Random.Next(Math.Min(MaxFoodItemsPerTick, MaxFoodItems - _food.Count));

            for (int i = 0; i < foodCount; i++)
            {
                var newFoodItem = new FoodItem(GenerateId())
                {
                    Energy = Random.Next(1, MaxEneryPerFoodItem + 1),
                };
                for (int j = 0; j < tryCount; j++)
                {
                    newFoodItem.Point = new Coord(Random.Next(Size.X), Random.Next(Size.Y));
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
    }
}
