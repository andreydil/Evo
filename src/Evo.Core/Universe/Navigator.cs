using System;
using System.Collections.Generic;
using System.Linq;
using Evo.Core.Basic;
using Evo.Core.Units;

namespace Evo.Core.Universe
{
    public class Navigator
    {
        private readonly World _world;
        private readonly Unit[,] _map;

        private readonly List<int> _verticalWalls = new List<int>();
        private readonly List<int> _horizontalWalls = new List<int>();

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

        public T FindUnit<T>(Coord point) where T : Unit
        {
            return FindUnit<T>(point.X, point.Y);
        }

        public T FindUnit<T>(int x, int y) where T: Unit
        {
            Unit unit = FindUnit(x, y);
            if (unit == null)
            {
                return null;
            }
            if (unit.Type != GetUnitType(typeof(T)))
            {
                return null;
            }
            return (T)unit;
        }
        
        private UnitType GetUnitType(Type type)
        {
            if (type == typeof (Individual))
            {
                return UnitType.Individual;
            }
            if (type == typeof (FoodItem))
            {
                return UnitType.Food;
            }
            throw new ArgumentOutOfRangeException($"Unknown UnitType {type.Name}");
        }

        public Individual FindIndividual(Coord point)
        {
            return FindUnit<Individual>(point);
        }

        public FoodItem FindFood(Coord point)
        {
            return FindUnit<FoodItem>(point);
        }
        
        public FoodItem FindClosestFood(Coord point, int sightRange)
        {
            return FindClosestUnit<FoodItem>(point, sightRange);
        }

        public Individual FindClosestIndividual(Coord point, int sightRange)
        {
            return FindClosestUnit<Individual>(point, sightRange);
        }

        private bool FastContains(List<int> list, int val)
        {
            for (int i = 0, length = list.Count; i < length; ++i)
            {
                if (list[i] == val)
                {
                    return true;
                }
            }
            return false;
        }

        private T FindClosestUnit<T>(Coord point, int sightRange) where T : Unit
        {
            int radius = 1;

            while (radius <= sightRange)
            {
                var topLeft = EnsureBounds(new Coord(point.X - radius, point.Y - radius));
                var bottomRight = EnsureBounds(new Coord(point.X + radius, point.Y + radius));
                topLeft = EnsureTopLeftPointWalls(topLeft, point);
                bottomRight = EnsureBottomRightPointWalls(bottomRight, point);
                int curX = topLeft.X;
                int curY = topLeft.Y;
                while (curX < bottomRight.X)   //right
                {
                    if (point.X != curX || point.Y != curY)
                    {
                        if (FastContains(_verticalWalls, curX))
                        {
                            --curX;
                            break;
                        }
                        var unit = FindUnit<T>(curX, curY);
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
                        if (FastContains(_horizontalWalls, curY))
                        {
                            --curY;
                            break;
                        }
                        var unit = FindUnit<T>(curX, curY);
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
                        if (FastContains(_verticalWalls, curX))
                        {
                            ++curX;
                            break;
                        }
                        var unit = FindUnit<T>(curX, curY);
                        if (unit != null)
                        {
                            return unit;
                        }
                    }
                    --curX;
                }
                while (curY > topLeft.Y)   //up
                {
                    if (point.X != curX || point.Y != curY)
                    {
                        if (FastContains(_horizontalWalls, curY))
                        {
                            ++curY;
                            break;
                        }
                        var unit = FindUnit<T>(curX, curY);
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
        
        public Coord BounceFromWalls(Coord point, Coord direction)
        {
            int x = direction.X;
            int y = direction.Y;
            bool bouncedX = false, bouncedY = false;
            if (direction.X == -1 && point.X <= 0)
            {
                x = 1;
                bouncedX = true;
            }
            if (direction.X == 1 && point.X >= _world.Size.X - 1)
            {
                x = -1;
                bouncedX = true;
            }
            if (direction.Y == -1 && point.Y <= 0)
            {
                y = 1;
                bouncedY = true;
            }
            if (direction.Y == 1 && point.Y >= _world.Size.Y - 1)
            {
                y = -1;
                bouncedY = true;
            }
            if (_verticalWalls.Contains(point.X + direction.X))
            {
                x = bouncedX ? 0 : -direction.X;
            }
            if (_horizontalWalls.Contains(point.Y + direction.Y))
            {
                y = bouncedY ? 0 : -direction.Y;
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

        public Coord EnsureTopLeftPointWalls(Coord topLeftPoint, Coord pointFrom)
        {
            var x = topLeftPoint.X;

            for (int i = 0; i < _verticalWalls.Count; i++)
            {
                var curX = _verticalWalls[i];
                if (curX >= pointFrom.X)
                {
                    break;
                }
                if (curX >= topLeftPoint.X)
                {
                    x = curX + 1;
                }
            }
            var y = topLeftPoint.Y;
            for (int i = 0; i < _horizontalWalls.Count; i++)
            {
                var curY = _horizontalWalls[i];
                if (curY >= pointFrom.Y)
                {
                    break;
                }
                if (curY >= topLeftPoint.Y)
                {
                    y = curY + 1;
                }
            }
            return new Coord(x, y);
        }

        public Coord EnsureBottomRightPointWalls(Coord bottomRightPoint, Coord pointFrom)
        {
            var x = bottomRightPoint.X;

            for (int i = 0; i < _verticalWalls.Count; i++)
            {
                var curX = _verticalWalls[i];
                if (curX <= pointFrom.X)
                {
                    break;
                }
                if (curX <= bottomRightPoint.X)
                {
                    x = curX - 1;
                }
            }
            var y = bottomRightPoint.Y;
            for (int i = 0; i < _horizontalWalls.Count; i++)
            {
                var curY = _horizontalWalls[i];
                if (curY <= pointFrom.Y)
                {
                    break;
                }
                if (curY <= bottomRightPoint.Y)
                {
                    y = curY - 1;
                }
            }
            return new Coord(x, y);
        }

        public Coord? PlaceChild(Coord fatherPoint, Coord motherPoint)
        {
            return PlaceNear(motherPoint) ?? PlaceNear(fatherPoint);
        }

        public bool IsWall(Coord point)
        {
            return _verticalWalls.Contains(point.X) || _horizontalWalls.Contains(point.Y);
        }

        public bool IsWall(WallType type, int coord)
        {
            return type == WallType.Vertical ? _verticalWalls.Contains(coord) : _horizontalWalls.Contains(coord);
        }
        
        public void AddWall(Wall wall)
        {
            if (wall.Type == WallType.Vertical)
            {
                _verticalWalls.Add(wall.Coord);
                for (int y = 0; y < _world.Size.Y; y++)
                {
                    _world.KillUnitByWall(wall.Coord, y);
                }
                _verticalWalls.Sort();
            }
            else
            {
                _horizontalWalls.Add(wall.Coord);
                for (int x = 0; x < _world.Size.X; x++)
                {
                    _world.KillUnitByWall(x, wall.Coord);
                }
                _horizontalWalls.Sort();
            }
        }
        
        public void RemoveWall(int x, int y)
        {
            _verticalWalls.Remove(x);
            _verticalWalls.Remove(y);
            _horizontalWalls.Remove(x);
            _horizontalWalls.Remove(y);
        }

        public IEnumerable<Wall> Walls => _verticalWalls.Select(c => new Wall(WallType.Vertical, c)).Concat(_horizontalWalls.Select(c => new Wall(WallType.Horizontal, c)));

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
