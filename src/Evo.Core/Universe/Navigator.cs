using System;
using System.Diagnostics;
using System.Linq;
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
                topLeft = EnsureTopLeftPointWalls(topLeft, point);
                bottomRight = EnsureBottomRightPointWalls(bottomRight, point);
                int curX = topLeft.X;
                int curY = topLeft.Y;
                while (curX < bottomRight.X)   //right
                {
                    if (point.X != curX || point.Y != curY)
                    {
                        if (_world.Walls.Vertical().Any(w => w.Coord == curX))  //TODO this may not work when it's the first iteration
                        {
                            --curX;
                            break;
                        }
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
                        if (_world.Walls.Horizontal().Any(w => w.Coord == curY))
                        {
                            --curY;
                            break;
                        }
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
                        if (_world.Walls.Vertical().Any(w => w.Coord == curX))
                        {
                            ++curX;
                            break;
                        }
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
                        if (_world.Walls.Horizontal().Any(w => w.Coord == curY))
                        {
                            ++curY;
                            break;
                        }
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
            if (_world.Walls.Vertical().Any(wall => wall.Coord == point.X + direction.X))
            {
                x = bouncedX ? 0 : -direction.X;
            }
            if (_world.Walls.Horizontal().Any(wall => wall.Coord == point.Y + direction.Y))
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
            var vertWalls = _world.Walls.Vertical().Where(w => w.Coord >= topLeftPoint.X && w.Coord < pointFrom.X).ToList();
            var x = vertWalls.Any() ? vertWalls.Max(w => w.Coord) + 1 : topLeftPoint.X;
            var horizWalls = _world.Walls.Horizontal().Where(w => w.Coord >= topLeftPoint.Y && w.Coord < pointFrom.Y).ToList();
            var y = horizWalls.Any() ? horizWalls.Max(w => w.Coord) + 1 : topLeftPoint.Y;
            return new Coord(x, y);
        }

        public Coord EnsureBottomRightPointWalls(Coord bottomRightPoint, Coord pointFrom)
        {
            var vertWalls = _world.Walls.Vertical().Where(w => w.Coord <= bottomRightPoint.X && w.Coord > pointFrom.X).ToList();
            var x = vertWalls.Any() ? vertWalls.Min(w => w.Coord) - 1 : bottomRightPoint.X;
            var horizWalls = _world.Walls.Horizontal().Where(w => w.Coord <= bottomRightPoint.Y && w.Coord > pointFrom.Y).ToList();
            var y = horizWalls.Any() ? horizWalls.Min(w => w.Coord) - 1 : bottomRightPoint.Y;
            return new Coord(x, y);
        }

        public Coord? PlaceChild(Coord fatherPoint, Coord motherPoint)
        {
            return PlaceNear(motherPoint) ?? PlaceNear(fatherPoint);
        }

        public bool IsWall(Coord point)
        {
            return _world.Walls.Any(w => w.Type == WallType.Vertical && w.Coord == point.X)
                   || _world.Walls.Any(w => w.Type == WallType.Horizontal && w.Coord == point.Y);
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
