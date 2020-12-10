using System.Numerics;
using EllipticCurve.Models;

namespace EllipticCurve.Interfaces
{
    public interface ICurveOperationManager<T>
    {
        Point<T> Add(Point<T> first, Point<T> second);
        Point<T> Multiple(Point<T> first, BigInteger k);
    }
}