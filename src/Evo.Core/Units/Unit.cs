using System;
using Evo.Core.Basic;

namespace Evo.Core.Units
{
    [Serializable]
    public abstract class Unit
    {
        public readonly ulong Id;
        public readonly UnitType Type;

        protected Unit(ulong id, UnitType type)
        {
            Id = id;
            Type = type;
        }

        public Coord Point { get; set; } = new Coord(0, 0);
    }
}
