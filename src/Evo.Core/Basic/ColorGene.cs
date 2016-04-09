using System;

namespace Evo.Core.Basic
{
    public class ColorGene : Gene
    {
        public ColorGene() : base(0x000000, 0xffffff)
        {
            Value = 0x808080;
        }

        public byte Red => (byte)(Value / 0x10000 % 0x100);
        public byte Green => (byte)(Value / 0x100 % 0x100);
        public byte Blue => (byte)(Value % 0x100);

        public void SetRGB(byte red, byte green, byte blue)
        {
            Value = red * 0x10000 + green * 0x100 + blue;
        }

        public double GetDifferenceFrom(ColorGene otherColor)
        {
            return Math.Sqrt((Red - otherColor.Red) * (Red - otherColor.Red) +
                             (Green - otherColor.Green) * (Green - otherColor.Green) + (
                                 Blue - otherColor.Blue) * (Blue - otherColor.Blue));
        }
    }
}
