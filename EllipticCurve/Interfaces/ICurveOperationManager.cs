using EllipticCurve.Models;
using Integer = System.Numerics.BigInteger;

namespace EllipticCurve.Interfaces
{
    public interface ICurveOperationManager<T>
    {
        Point<T> Add(Point<T> first, Point<T> second);
        Point<T> Multiple(Point<T> first, Integer k);
    }
}