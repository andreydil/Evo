using System.Collections;
using System.Collections.Generic;
using Evo.Core.Basic;

namespace Evo.Core.Units
{
    public class Unit
    {
        public readonly ulong Id;

        public Unit(ulong id)
        {
            Id = id;
        }

        public Coord Point { get; set; } = new Coord(0, 0);
    }
}
