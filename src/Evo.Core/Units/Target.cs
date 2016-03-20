using Evo.Core.Basic;

namespace Evo.Core.Units
{
    public class Target
    {
        public TargetType TargetType { get; set; }
        public ulong Id { get; set; }
        public Coord Direction { get; set; } = new Coord(0, 0);

        public override string ToString()
        {
            return $"{TargetType} {Id} at {Direction}";
        }
    }
}
