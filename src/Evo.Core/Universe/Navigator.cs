using System;
using System.Diagnostics;
using Evo.Core.Basic;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    public class Navigator
    {
        private readonly World _world;
        private readonly Unit[,] _map;

        public Navigator(World world)
        {
            _world = world;
            _map = new Unit[world.Size.X, world.Size.Y];
        }

        public void PutUnit(Unit unit)
        {
            if (FindUnit(unit.Point) != null)
            {
                throw new ArgumentOutOfRangeException($"There is already a unit in coordinates {unit.Point}");
            }
            _map[unit.Point.X, unit.Point.Y] = unit;
        }

        public void MoveUnit(Unit unit, Coord newPoint)
        {
            _map[unit.Point.X, unit.Point.Y] = null;
            unit.Point = newPoint;
            PutUnit(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            var unitAtPoint = FindUnit(unit.Point);
            if (unitAtPoint != null && unitAtPoint.Id == unit.Id)
            {
                _map[unit.Point.X, unit.Point.Y] = null;
            }
        }
        
        public Unit FindUnit(Coord point)
        {
            return FindUnit(point.X, point.Y);
        }

        public Unit FindUnit(int x, int y)
        {
            return _map[x, y];
        }

        public Individual FindIndividual(Coord point)
        {
            return FindUnit(point) as Individual;
        }

        public FoodItem FindFood(Coord point)
        {
            return FindUnit(point) as FoodItem;
        }
        
        public FoodItem FindClosestFood(Coord point, int sightRange)
        {
            return FindClosestUnit<FoodItem>(point, sightRange);
        }

        public Individual FindClosestIndividual(Coord point, int sightRange)
        {
            return FindClosestUnit<Individual>(point, sightRange);
        }

        private T FindClosestUnit<T>(Coord point, int sightRange) where T : Unit
        {
            int radius = 1;

            while (radius <= sightRange)
            {
                var topLeft = EnsureBounds(new Coord(point.X - radius, point.Y - radius));
                var bottomRight = EnsureBounds(new Coord(point.X + radius, point.Y + radius));
                int curX = topLeft.X;
                int curY = topLeft.Y;
                while (curX < bottomRight.X)   //right
                {
                    if (point.X != curX || point.Y != curY)
                    {
                        var unit = FindUnit(curX, curY) as T;
                        if (unit != null)
                        {
                            return unit;
                        }
                    }
                    ++curX;
                }
                while (curY < bottomRight.Y)   //down
                {
                    if (point.X != curX || point.Y != curY)
                    {
                        var unit = FindUnit(curX, curY) as T;
                        if (unit != null)
                        {
                            return unit;
                        }
                    }
                    ++curY;
                }
                while (curX > topLeft.X)   //left
                {
                    if (point.X != curX || point.Y != curY)
                    {
                        var unit = FindUnit(curX, curY) as T;
                        if (unit != null)
                        {
                            return unit;
                        }
                    }
                    --curX;
                }
                while (curY > bottomRight.Y)   //up
                {
                    if (point.X != curX || point.Y != curY)
                    {
                        var unit = FindUnit(curX, curY) as T;
                        if (unit != null)
                        {
                            return unit;
                        }
                    }
                    --curY;
                }

                ++radius;
            }
            return null;
        }
        
        public Coord BounceFromMapBorders(Coord point, Coord direction)
        {
            int x = direction.X;
            int y = direction.Y;
            if (direction.X == -1 && point.X <= 0)
            {
                x = 1;
            }
            if (direction.X == 1 && point.X >= _world.Size.X - 1)
            {
                x = -1;
            }
            if (direction.Y == -1 && point.Y <= 0)
            {
                y = 1;
            }
            if (direction.Y == 1 && point.Y >= _world.Size.Y - 1)
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
