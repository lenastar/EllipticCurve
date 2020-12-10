using EllipticCurve.Helpers;
using EllipticCurve.Models;
using ExtendedArithmetic;

namespace EllipticCurve.Managers
{
    public class GfCurveSuperSingularManager  : CurveOperationManager<Polynomial>
    {
        private readonly GfFieldHelper _gfFieldHelper;
        private readonly Polynomial _k;
        private readonly Polynomial _a;
        private readonly Polynomial _b;
        private readonly Polynomial _c;

        public GfCurveSuperSingularManager(long order, Polynomial a, Polynomial b, Polynomial c)
        {
            _k = Polynomial.Parse(GfFieldHelper.IrreduciblePolynomials[order]);
            _a = a;
            _b = b;
            _c = c;
            _gfFieldHelper = new GfFieldHelper(_k);
        }

        public override Point<Polynomial> Add(Point<Polynomial> first, Point<Polynomial> second)
        {
            if (CheckInfinityPoints(first, second, out var result))
            {
                return result;
            }

            Polynomial coefficient;
            
            if (first.X.Equals(second.X))
            {
                if (second.Y.Equals(_a.Add(first.Y)))
                {
                    return Point<Polynomial>.Infinity;
                }

                coefficient = Polynomial.Square(first.X)
                    .Add(_b)
                    .Multiply(_gfFieldHelper.Invert(_a))
                    .Mod(_k);
            }
            else
            {
                coefficient = first.Y
                    .Add(second.Y)
                    .Multiply(_gfFieldHelper.Invert(first.X.Add(second.X)))
                    .Mod(_k);
            }

            var x3 = Polynomial
                .Sum(Polynomial.Square(coefficient), _a.Multiply(coefficient), _b, first.X, second.X)
                .Mod(_k);
            
            // из уравнения секущей
            var y3 = first.Y
                .Add(coefficient.Multiply(x3.Add(first.X)))
                .Mod(_k);

            var resY = _a
                .Add(y3)
                .Mod(_k);
            
            // x = x3, y = _a * y3 по теореме Виета
            return new Point<Polynomial>(x3, resY);
        }
    }
}