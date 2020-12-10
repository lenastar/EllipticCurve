using System.Collections.Generic;
using ExtendedArithmetic;

namespace EllipticCurve.Helpers
{
    public class GfFieldHelper
    {
        private readonly Polynomial _k;

        public static Dictionary<long, string> IrreduciblePolynomials = new Dictionary<long, string>
        {
            {2, "x^2+x+1"},
            {3, "x^3+x+1"},
            {4, "x^4+x+1"},
            {5, "x^5+x^2+1"},
            {6, "x^6+x+1"},
            {7, "x^7+x^3+1"},
            {8, "x^8+x^4+x^3+x^2+1"},
            {9, "x^9+x^4+1"},
            {10, "x^10+x^3+1"},
            {11, "x^11+x^2+1"},
            {12, "x^12+x^6+x^4+x+1"},
            {13, "x^13+x^4+x^3+x+1"},
            {14, "x^14+x^10+x^6+x+1"},
            {15, "x^15+x+1"},
            {16, "x^16+x^12+x^3+x+1"},
            {17, "x^17+x^3+1"},
            {18, "x^18+x^7+1"},
            {19, "x^19+x^5+x^2+x+1"},
            {20, "x^20+x^3+1"},
            {163, "x^163+x^7+x^6+x^3+1"},
            {233, "x^233+x^74+1"},
            {283, "x^283+x^12+x^7+x^5+1"},
            {409, "x^409+x^87+1"},
            {571, "x^571+x^10+x^5 +x^2+1"}
        };

            public GfFieldHelper(Polynomial k)
            {
            _k = k;
        }

        public Polynomial Invert(Polynomial element)
        {
            ExtendedGCD(element, _k, out var result, out _);
            return Polynomial.Field.Modulus(result, 2);
        }

        private Polynomial ExtendedGCD(Polynomial a, Polynomial b, out Polynomial x, out Polynomial y)
        {
            if (a.Equals(Polynomial.Zero))
            {
                x = Polynomial.Zero;
                y = Polynomial.One;
                return b;
            }

            var d = ExtendedGCD(b.Mod(a), a, out var x1, out var y1);

            x = y1.Subtract(b.Divide(a).Multiply(x1));
            y = x1;

            return d;
        }
    }
}