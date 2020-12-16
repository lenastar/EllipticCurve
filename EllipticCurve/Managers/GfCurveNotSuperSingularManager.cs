using EllipticCurve.Helpers;
using EllipticCurve.Models;
using ExtendedArithmetic;

namespace EllipticCurve.Managers
{
    public class GfCurveNotSuperSingularManager : CurveOperationManager<Polynomial>
    {
        private readonly Polynomial _k;
        private readonly Polynomial _a;
        private readonly Polynomial _b;
        private readonly Polynomial _c;
        
        public GfCurveNotSuperSingularManager(long order, Polynomial a, Polynomial b, Polynomial c)
        {
            _k = Polynomial.Parse(PolynomialExtensions.IrreduciblePolynomials[order]);
            _a = a;
            _b = b;
            _c = c;
        }

        public override Point<Polynomial> Add(Point<Polynomial> first, Point<Polynomial> second)
        {
            if (CheckInfinityPoints(first, second, out var result))
            {
                return result;
            }

            Polynomial multiplier, inverted;

            if (first.X.Equals(second.X.Mod(_k)))
            {
                if (second.Y.Equals(_a.Multiply(first.X).Add(first.Y).Mod(_k)))
                {
                    return Point<Polynomial>.Infinity;
                }

                inverted = _a.Multiply(first.X).Invert(_k);
                multiplier = Polynomial
                    .Square(first.X)
                    .Add(_a.Multiply(first.Y))
                    .Mod(_k);
            }
            else
            {
                inverted =  first.X.Add(second.X).Invert(_k);
                multiplier = first.Y.Add(second.Y).Mod(_k);
            }

            var coefficient = multiplier.Multiply(inverted).Mod(_k);
            
            var x3 = Polynomial
                .Sum(Polynomial.Square(coefficient), _a.Multiply(coefficient), _b, first.X, second.X)
                .Mod(_k);
            var y3 = first.Y
                .Add(coefficient.Multiply(x3.Add(first.X)))
                .Mod(_k);

            var resY = _a
                .Multiply(x3)
                .Add(y3)
                .Mod(_k);
            
            return new Point<Polynomial>(x3, resY);
        }
    }
}