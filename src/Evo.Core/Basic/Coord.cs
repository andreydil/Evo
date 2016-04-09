using System;

namespace Evo.Core.Basic
{
    public struct Coord : IEquatable<Coord>
    {
        public int X;
        public int Y;

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static Coord operator +(Coord p1, Coord p2)
        {
            return new Coord(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Coord operator -(Coord p1, Coord p2)
        {
            return new Coord(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static bool operator ==(Coord p1, Coord p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Coord p1, Coord p2)
        {
            return !(p1 == p2);
        }

        #region IEquatable implementation
        
        private bool Equals(Coord other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj.GetType() == this.GetType() && Equals((Coord)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        bool IEquatable<Coord>.Equals(Coord other)
        {
            return this.Equals(other);
        }

        #endregion

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public double GetDistanceTo(Coord to)
        {
            var diff = to - this;
            return Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
        }

        public Coord FindDirection(Coord to)
        {
            var vector = to - this;
            if (vector.X != 0)
            {
                vector.X = vector.X / Math.Abs(vector.X);
            }
            if (vector.Y != 0)
            {
                vector.Y = vector.Y / Math.Abs(vector.Y);
            }
            return vector;
        }
    }
}
