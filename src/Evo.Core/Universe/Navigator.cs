using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Evo.Core.Basic;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    public class Navigator
    {
        private readonly World _world;

        public Navigator(World world)
        {
            _world = world;
        }

        public Unit FindUnit(Coord point)
        {
            return FindUnit(point, _world.Population) ?? FindUnit(point, _world.Food);
        }

        public Individual FindIndividual(Coord point)
        {
            return (Individual)FindUnit(point, _world.Population);
        }

        public FoodItem FindFood(Coord point)
        {
            return (FoodItem)FindUnit(point, _world.Food);
        }

        private Unit FindUnit<T>(Coord point, IList<T> list) where T : Unit
        {
            return list.FirstOrDefault(u => u.Point == point);
        }

        public FoodItem FindClosestFood(Coord point, int sightRange)
        {
            return (FoodItem)FindClosestUnit(point, _world.Food, sightRange);
        }

        public Individual FindClosestIndividual(Coord point, int sightRange)
        {
            return (Individual)FindClosestUnit(point, _world.Population, sightRange);
        }

        private Unit FindClosestUnit<T>(Coord point, IList<T> list, int sightRange) where T : Unit
        {
            var allUnitsInSightRange = FindAllFoodInRadius(point, list, sightRange);
            int radius = 1;

            while (radius <= sightRange)
            {
                var foodInRadius = FindAllFoodInRadius(point, allUnitsInSightRange, radius);
                if (foodInRadius.Any())
                {
                    return foodInRadius[_world.Random.Next(foodInRadius.Count)];
                }
                ++radius;
            }
            return null;
        }

        private List<T> FindAllFoodInRadius<T>(Coord point, IList<T> list, int radius) where T : Unit
        {
            return list.Where(f => f.Point.X >= point.X - radius && f.Point.X <= point.X + radius
                                   && f.Point.Y >= point.Y - radius && f.Point.Y <= point.Y + radius
                                   && f.Point != point).ToList();
        }

        public Coord BounceFromMapBorders(Coord point, Coord direction)
        {
            int x = direction.X;
            int y = direction.Y;
            if (direction.X == -1 && point.X <= 0)
            {
                x = 1;
            }
            if (direction.X == 1 && point.X >= _world.Size.X)
            {
                x = -1;
            }
            if (direction.Y == -1 && point.Y <= 0)
            {
                y = 1;
            }
            if (direction.Y == 1 && point.Y >= _world.Size.Y)
            {
                y = -1;
            }
            return new Coord(x, y);
        }

        public Coord EnsureBounds(Coord point)
        {
            if (point.X < 0)
            {
                point.X = 0;
            }
            else if (point.X >= _world.Size.X)
            {
                point.X = _world.Size.X - 1;
            }
            if (point.Y < 0)
            {
                point.Y = 0;
            }
            else if (point.Y >= _world.Size.Y)
            {
                point.Y = _world.Size.Y - 1;
            }

            return point;
        }

        public Coord? PlaceChild(Coord fatherPoint, Coord motherPoint)
        {
            return PlaceNear(motherPoint) ?? PlaceNear(fatherPoint);
        }

        private Coord? PlaceNear(Coord point)
        {
            for (int deltaX = -1; deltaX <= 1; deltaX++)
            {
                for (int deltaY = -1; deltaY <= 1; deltaY++)
                {
                    if (deltaX == 0 & deltaY == 0)
                    {
                        continue;
                    }

                    var curPoint = new Coord(point.X + deltaX, point.Y + deltaY);

                    if (curPoint.X < 0 || curPoint.X >= _world.Size.X || curPoint.Y < 0 || curPoint.Y >= _world.Size.Y)
                    {
                        continue;
                    }

                    if (FindUnit(curPoint) == null)
                    {
                        return curPoint;
                    }
                }
            }
            return null;
        }
    }
}
