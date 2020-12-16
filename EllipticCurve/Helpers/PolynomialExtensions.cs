using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EllipticCurve.Enums;
using ExtendedArithmetic;
using Integer = System.Numerics.BigInteger;


namespace EllipticCurve.Helpers
{
    public static class PolynomialExtensions 
    {
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
        public static Polynomial Parse(string input)
        {
            input = IoHelper.ChangeNotationToDecimal(input);

            var terms = BigInteger
                .Parse(input)
                .ConvertToBase2()
                .Select((x, i) => new Term(x ? 1 : 0, i))
                .ToArray();
            
            return new Polynomial(terms);
        }

        public static string ToNumberString(this Polynomial poly, NumericSystem system)
        {
            if (poly == null)
            {
                return string.Empty;
            }
            
            Integer? result = 0;
            foreach (var term in poly.Terms)
            {
                if (!term.CoEfficient.IsEven)
                {
                    result += (Integer) Math.Pow(2, term.Exponent);
                }
            }

            return result
                .ToNumberString(system);
        }

        public static Polynomial Multiply(this Polynomial left, Polynomial right)
        {
            return Polynomial.Multiply(left, right);
        }
        
        public static Polynomial Add(this Polynomial left, Polynomial right)
        {
            return Polynomial.Add(left, right);
        }
        
        public static Polynomial Invert(this Polynomial a, Polynomial order)
        {
            Polynomial s = Polynomial.Zero;
            Polynomial prevS = Polynomial.One;

            var r = order;
            var prevR = a;

            while (!r.Equals(Polynomial.Zero))
            {
                var q = prevR.Divide(r);

                var temp = prevR;
                prevR = r;
                r = Mod(temp.Subtract(q.Multiply(r)), order);

                var tempS = prevS;
                prevS = s;
                s = tempS.Subtract(q.Multiply(s));
            }

            return Mod(prevS, order);
        }
        
        public static Polynomial Mod(this Polynomial a, Polynomial order)
        {
            return ModMod(ModMod(a, order).Add(order), order);
        }
        
        private static Polynomial Divide(this Polynomial left, Polynomial right)
        {
            return Polynomial.Divide(left, right);
        }

        private static Polynomial Subtract(this Polynomial left, Polynomial right)
        {
            return Polynomial.Subtract(left, right);
        }

        private static Polynomial ModMod(Polynomial left, Polynomial right)
        {
            return Polynomial.Field.ModMod(left, right, 2);
        }
    }
}