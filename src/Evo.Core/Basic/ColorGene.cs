using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Basic
{
    public class ColorGene : Gene
    {
        public ColorGene() : base(0x000000, 0xffffff)
        {
        }

        public byte Red => (byte)(Value / 0x10000 % 0x100);
        public byte Green => (byte)(Value / 0x100 % 0x100);
        public byte Blue => (byte)(Value % 0x100);

        public double GetDifferenceFrom(ColorGene otherColor)
        {
            return Math.Sqrt((Red - otherColor.Red) * (Red - otherColor.Red) + 
                         (Green - otherColor.Green) * (Green - otherColor.Green) + (
                            Blue - otherColor.Blue) *(Blue - otherColor.Blue));
        }
    }
}
