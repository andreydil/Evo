using System;

namespace Evo.Core.Basic
{
    public class LimitedInt
    {
        private int _value;

        public LimitedInt(int min, int max)
        {
            if (max < min)
            {
                throw new ArgumentOutOfRangeException("Min must be less than max");
            }
            Min = min;
            Max = max;
            _value = (max + min) / 2;
        }

        public int Min { get; }
        public int Max { get; }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value <= Min)
                {
                    _value = Min;
                }
                else if (value >= Max)
                {
                    _value = Max;
                }
                else
                {
                    _value = value;
                }
            }
        }

        public double NormalizedValue => (double) Value / (Max - Min);

        public static implicit operator int(LimitedInt x)
        {
            return x.Value;
        }

        public static int operator +(LimitedInt x, LimitedInt y)
        {
            return x.Value + y.Value;
        }

        public static int operator -(LimitedInt x, LimitedInt y)
        {
            return x.Value - y.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
