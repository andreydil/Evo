using System;
using Evo.Core.Basic;

namespace Evo.Core.Units
{
    [Serializable]
    public abstract class Unit
    {
        public readonly ulong Id;

        protected Unit(ulong id)
        {
            Id = id;
        }

        public Coord Point { get; set; } = new Coord(0, 0);

        public abstract UnitType Type { get; }
    }
}
