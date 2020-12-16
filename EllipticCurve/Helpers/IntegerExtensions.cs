using System;
using System.Linq;
using EllipticCurve.Enums;
using ExtendedArithmetic;
using Integer = System.Numerics.BigInteger;
namespace EllipticCurve.Helpers
{
    public static class IntegerExtensions
    {
        public static Polynomial Parse(this Integer input)
        {
            var terms = input
                .ConvertToBase2()
                .Select((x, i) => new Term(x ? 1 : 0, i))
                .ToArray();
            
            return new Polynomial(terms);
        }

        public static string ToNumberString(this Integer? input, NumericSystem system)
        {
            if (!input.HasValue)
            {
                return string.Empty;
            }

            return input.Value.ToNumberString(system);
        }
        
        public static string ToNumberString(this Integer input, NumericSystem system)
        {
            switch (system)
            {
                case NumericSystem.Decimal:
                    return input.ToString();
                case NumericSystem.Hex:
                    return "0x" + input.ToString("x");
                default:
                    throw new ArgumentOutOfRangeException(nameof(system), system, null);
            }
        }
    }
}