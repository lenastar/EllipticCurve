using EllipticCurve.Models;
using ExtendedArithmetic;
using Integer = System.Numerics.BigInteger;

namespace EllipticCurve.Managers
{
    public class ZpCurveOperationManager : CurveOperationManager<Integer?>
    {
        private readonly Integer _p;
        private readonly Integer _a;
        private Integer _b;
        
        public ZpCurveOperationManager(Integer p, Integer a, Integer b)
        {
            _p = p;
            _a = Mod(a);
            _b = Mod(b);
        }

        public override Point<Integer?> Add(Point<Integer?> first, Point<Integer?> second)
        {
            if (CheckInfinityPoints(first, second, out var result))
            {
                return result;
            }

            Integer coefficient;

            if (Mod(first.X.Value) == Mod(second.X.Value))
            {
                if (Mod(first.Y.Value) == 0 && Mod(second.Y.Value) == 0)
                {
                    return Point<Integer?>.Infinity;
                }

                if (Mod(first.Y.Value + second.Y.Value) == 0)
                {
                    return Point<Integer?>.Infinity;
                }
                
                var inverted = Invert(2 * first.Y.Value);
                coefficient = Mod((3 * first.X.Value * first.X.Value + _a) * inverted);
            }
            else
            {
                var inverted = Invert(second.X.Value - first.X.Value);
                coefficient = Mod((second.Y.Value - first.Y.Value) * inverted);
            }

            var x3 = Mod(coefficient * coefficient - first.X.Value - second.X.Value);
            var y3 = Mod(first.Y.Value + coefficient * (x3 - first.X.Value));
            
            return new Point<Integer?>(x3, Mod(-y3) );
        }

        private Integer Invert(Integer a)
        {
            Integer s = 0;
            Integer prevS = 1;

            var r = _p;
            var prevR = Mod(a);

            while (r != 0)
            {
                var q = prevR / r;

                var temp = prevR;
                prevR = r;
                r = temp - q * r;

                var tempS = prevS;
                prevS = s;
                s = tempS - q * s;
            }

            return Mod(prevS);
        }
        
        private Integer Mod(Integer a)
        {
            return ((a % _p) + _p) % _p;
        }
    }
}