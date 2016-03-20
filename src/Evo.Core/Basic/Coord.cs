using System;

namespace Evo.Core.Basic
{
    public class Coord : IEquatable<Coord>
    {
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
        
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
            if (ReferenceEquals(p1, p2))
            {
                return true;
            }
            
            if (((object)p1 == null) || ((object)p2 == null))
            {
                return false;
            }
            return p1.Equals(p2);
        }

        public static bool operator !=(Coord p1, Coord p2)
        {
            return !(p1 == p2);
        }

        #region IEquatable implementation

        protected bool Equals(Coord other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
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
