using EllipticCurve.Interfaces;
using EllipticCurve.Models;
using Integer = System.Numerics.BigInteger;

namespace EllipticCurve.Managers
{
    public abstract class CurveOperationManager<T>: ICurveOperationManager<T> 
    {
        public abstract Point<T> Add(Point<T> first, Point<T> second);

        protected bool CheckInfinityPoints(Point<T> first, Point<T> second, out Point<T> result)
        {
            if (first.IsInfinity())
            {
                result = second;
                return true;
            }

            if (second.IsInfinity())
            {
                result = first;
                return true;
            }

            result = Point<T>.Infinity;
            return false;
        }

        public Point<T> Multiple(Point<T> first, Integer k)
        {
            var result = Point<T>.Infinity;
            var addend = first;

            while (k != 0)
            {
                if ((k & 1) == 1)
                {
                    result = Add(result, addend);
                }

                addend = Add(addend, addend);

                k >>= 1;
            }

            return result;
        }
    }
}