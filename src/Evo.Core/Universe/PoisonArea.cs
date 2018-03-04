using System;
using Evo.Core.Basic;

namespace Evo.Core.Universe
{
    [Serializable]
    public class PoisonArea
    {
        public Coord TopLeft { get; set; }
        public Coord BottomRight { get; set; }
        public int Intensity { get; set; }
    }
}
