using System.Collections.Generic;
using System.Linq;

namespace Evo.Core.Units
{
    public struct Wall
    {
        public WallType Type;
        public int Coord;

        public Wall(WallType type, int coord)
        {
            Type = type;
            Coord = coord;
        }
    }

    public static class WallExtensions
    {
        public static IEnumerable<Wall> Vertical(this IEnumerable<Wall> walls)
        {
            return walls.Where(w => w.Type == WallType.Vertical);
        }

        public static IEnumerable<Wall> Horizontal(this IEnumerable<Wall> walls)
        {
            return walls.Where(w => w.Type == WallType.Horizontal);
        }
    }
}
