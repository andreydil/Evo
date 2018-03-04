using System;

namespace Evo.Core.Basic
{
    [Serializable]
    public class LimitedDouble
    {
        private double _value;

        public LimitedDouble(double min, double max)
        {
            if (max < min)
            {
                throw new ArgumentOutOfRangeException("Min must be less than max");
            }
            Min = min;
            Max = max;
            _value = (max + min) / 2;
        }

        public double Min { get; }
        public double Max { get; }

        public double Value
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

        public double NormalizedValue => (double)Value / (Max - Min);

        public static implicit operator double(LimitedDouble x)
        {
            return x.Value;
        }

        public static double operator +(LimitedDouble x, LimitedDouble y)
        {
            return x.Value + y.Value;
        }

        public static double operator -(LimitedDouble x, LimitedDouble y)
        {
            return x.Value - y.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
