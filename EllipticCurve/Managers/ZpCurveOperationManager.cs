using System.Numerics;
using EllipticCurve.Models;
using ExtendedArithmetic;

namespace EllipticCurve.Managers
{
    public class ZpCurveOperationManager : CurveOperationManager<BigInteger?>
    {
        private readonly BigInteger _p;
        private readonly BigInteger _a;
        private BigInteger _b;
        
        public ZpCurveOperationManager(BigInteger p, BigInteger a, BigInteger b)
        {
            _p = p;
            _a = a;
            _b = b;
        }

        public override Point<BigInteger?> Add(Point<BigInteger?> first, Point<BigInteger?> second)
        {
            if (CheckInfinityPoints(first, second, out var result))
            {
                return result;
            }

            BigInteger coefficient;

            if (first.X == second.X)
            {
                /*if (first.Y == 0 && second.Y == 0)
                {
                    return Point<BigInteger?>.Infinity;
                }*/

                if (first.Y + second.Y == 0)
                {
                    return Point<BigInteger?>.Infinity;
                }
                
                var inverted = Polynomial.Algorithms.ModularMultiplicativeInverse(2 * first.Y.Value, _p);

                coefficient = (3 * first.X.Value * first.X.Value + _a) * inverted % _p;
            }
            else
            {
                var inverted = Polynomial.Algorithms.ModularMultiplicativeInverse(second.X.Value - first.X.Value, _p);
                coefficient = (second.Y.Value - first.Y.Value) * inverted % _p;
            }

            var x3 = (coefficient * coefficient - first.X.Value - second.X.Value) % _p;
            var y3 = (first.Y + coefficient * (x3 - first.X.Value)) % _p;
            
            return new Point<BigInteger?>(x3, -y3 % _p);
        }
    }
}