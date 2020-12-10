
using ExtendedArithmetic;

namespace EllipticCurve.Helpers
{
    public static class PolynomialExtensions 
    {
        public static Polynomial Multiply(this Polynomial left, Polynomial right)
        {
            return Polynomial.Multiply(left, right);
        }
        
        public static Polynomial Add(this Polynomial left, Polynomial right)
        {
            return Polynomial.Add(left, right);
        }
        
        public static Polynomial Divide(this Polynomial left, Polynomial right)
        {
            return Polynomial.Divide(left, right);
        }
        
        public static Polynomial Mod(this Polynomial left, Polynomial right)
        {
            return Polynomial.Field.ModMod(left, right, 2);
        }

        public static Polynomial Subtract(this Polynomial left, Polynomial right)
        {
            return Polynomial.Subtract(left, right);
        }
    }
}