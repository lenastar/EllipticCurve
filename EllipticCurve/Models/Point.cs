using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EllipticCurve.Models
{
    public class Point<T> : IEquatable<Point<T>>
    {
        public T X { get; }
        public T Y { get; }
        
        public static Point<T> Infinity => new Point<T>(default, default);

        public bool IsInfinity() => X == null && Y == null;

        public Point(T x, T y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X?.ToString()}, {Y?.ToString()})";
        }

        public bool Equals(Point<T> other)
        {
            return other != null && EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point<T>) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}