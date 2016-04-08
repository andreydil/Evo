﻿using System;
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
        private ulong _idGenerator = 0;
        public readonly Mutator Mutator;
        public readonly Navigator Navigator;
        public readonly DecisionMaker DecisionMaker;
        public readonly Coord Size;
        private readonly StatCounter _statCounter;

        public World(Random random, Coord size)
        {
            Random = random;
            Size = size;
            Mutator = new Mutator(this);
            Navigator = new Navigator(this);
            DecisionMaker = new DecisionMaker(this);
            _statCounter = new StatCounter();
        }

        public LimitedInt MutationProbability { get; set; } = new LimitedInt(1, Constants.Probability100Percent);
        public LimitedInt MutationMaxDelta { get; set; } = new LimitedInt(1, 1000);
        public LimitedInt MaxFoodItemsPerTick { get; set; } = new LimitedInt(1, 10000);
        public LimitedInt MaxEneryPerFoodItem { get; set; } = new LimitedInt(1, 100);
        public LimitedInt MaxFoodItems { get; set; } = new LimitedInt(1, 10000);
        public LimitedInt EnergyDrainModificator { get; set; } = new LimitedInt(1, 1000);

        public ulong Tick { get; private set; } = 0;
        public readonly List<Individual> Population = new List<Individual>();
        public readonly List<FoodItem> Food = new List<FoodItem>();

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
            SpreadFood();
            foreach (var individual in Population.ToList())
            {
                if (!individual.IsAlive)
                {
                    Population.Remove(individual);
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
                individual.LiveOneTick();
            }
            PopulationSize.Add(Tick, Population.Count);
            FoodAmount.Add(Tick, Food.Count);
            ++Tick;
        }

        public void AddIndividual(Individual individual)
        {
            if (Navigator.FindUnit(individual.Point) != null)
            {
                throw new ArgumentOutOfRangeException($"There is already a unit in coordinates {individual.Point}");
            }
            Population.Add(individual);
        }

        public void AddIndividuals(IEnumerable<Individual> individuals)
        {
            foreach (var individual in individuals)
            {
                if (Navigator.FindUnit(individual.Point) != null)
                {
                    throw new ArgumentOutOfRangeException($"There is already a unit in coordinates {individual.Point}");
                }
                Population.Add(individual);
            }
        }

        public void AddFood(FoodItem foodItem)
        {
            if (Navigator.FindUnit(foodItem.Point) != null)
            {
                return; //TODO: investigate why
                //throw new ArgumentOutOfRangeException($"There is already a unit in coordinates {foodItem.Point}");
            }
            Food.Add(foodItem);
        }

        public bool CheckRng(LimitedInt range)
        {
            return CheckRng(range.Value, range.Min, range.Max);
        }

        public bool CheckRng(int value, int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue) <= value;
        }

        public void SpreadFood()
        {
            if (Food.Count >= MaxFoodItems)
            {
                return;
            }

            const int tryCount = 100;
            var foodCount = Random.Next(Math.Min(MaxFoodItemsPerTick, MaxFoodItems - Food.Count));

            for (int i = 0; i < foodCount; i++)
            {
                var newFoodItem = new FoodItem(GenerateId())
                {
                    Energy = Random.Next(1, MaxEneryPerFoodItem),
                };
                for (int j = 0; j < tryCount; j++)
                {
                    newFoodItem.Point.X = Random.Next(Size.X);
                    newFoodItem.Point.Y = Random.Next(Size.Y);
                    var unitInPoint = Navigator.FindUnit(newFoodItem.Point);
                    if (unitInPoint == null)
                    {
                        AddFood(newFoodItem);
                        break;
                    }

                    var induvidual = unitInPoint as Individual;
                    if (induvidual != null)
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
                    individual.Point.X = Random.Next(topLeftPoint.X, bottomRightPoint.X);
                    individual.Point.Y = Random.Next(topLeftPoint.Y, bottomRightPoint.Y);
                    Navigator.InsureBounds(individual.Point);
                    if (Navigator.FindIndividual(individual.Point) != null)
                    {
                        continue;
                    }

                    Population.Add(individual);
                    break;
                }
            }
        }

        public Individual AverageIndividual => _statCounter.GetAverage(Population);
    }
}
