using System.Linq;
using System.Numerics;
using EllipticCurve.Enums;
using EllipticCurve.Helpers;
using EllipticCurve.Models;
using ExtendedArithmetic;
using NUnit.Framework;

namespace EllipticCurveTest
{
    public class IoHelperTests
    {
        [Test]
        [TestCase(1, 2, 3, 4, 5, 6, "(1, 2) + (3, 4) = (5, 6)" )]
        [TestCase(null, null, 3, 4,  null, null, "(, ) + (3, 4) = (, )" )]
        public void FormatAddLongTest(long? x1, long? y1, long? x2, long? y2, long? resX, long? resY, string expected)
        {
            var left = new Point<BigInteger?>(x1, y1);
            var right = new Point<BigInteger?>(x2, y2);
            var result = new Point<BigInteger?>(resX, resY);
            Assert.AreEqual(expected, IoHelper.FormatAdd(left, right, result, IntegerExtensions.ToNumberString));
        }
        
        [Test]
        [TestCase(1, 2, 3, 5, 6, "(1, 2) * 3 = (5, 6)" )]
        [TestCase(null, null, 3,  null, null, "(, ) * 3 = (, )" )]
        public void FormatMulLongTest(long? x1, long? y1, int k, long? resX, long? resY, string expected)
        {
            var left = new Point<BigInteger?>(x1, y1);
            var result = new Point<BigInteger?>(resX, resY);
            Assert.AreEqual(expected, IoHelper.FormatMul(left, k.ToString(), result, IntegerExtensions.ToNumberString));
        }
        
        [Test]
        [TestCase("1", "2", "3", "4", "5", "6", "(1, 2) + (3, 4) = (5, 6)" )]
        [TestCase(null, null, "3", "4",  null, null, "(, ) + (3, 4) = (, )" )]
        public void FormatAddPolynomialTest(string x1, string y1, string x2, string y2, string resX, string resY, string expected)
        {
            var left = x1 != null && y1 != null ? new Point<Polynomial>(PolynomialExtensions.Parse(x1), PolynomialExtensions.Parse(y1)) : 
            Point<Polynomial>.Infinity;
            var right = x2 != null && y2 != null ? new Point<Polynomial>(PolynomialExtensions.Parse(x2), PolynomialExtensions.Parse(y2)): 
            Point<Polynomial>.Infinity;
            var result = resX != null && resY != null ? new Point<Polynomial>(PolynomialExtensions.Parse(resX), PolynomialExtensions.Parse(resY)) : 
            Point<Polynomial>.Infinity;
            Assert.AreEqual(expected, IoHelper.FormatAdd(left, right, result, PolynomialExtensions.ToNumberString));
        }
        
        [Test]
        [TestCase("1", "2", 3, "5", "6", "(1, 2) * 3 = (5, 6)" )]
        [TestCase(null, null, 3, null, null, "(, ) * 3 = (, )" )]
        public void FormatMulPolynomialTest(string x1, string y1, int k, string resX, string resY, string expected)
        {
            var left = x1 != null && y1 != null ? new Point<Polynomial>(PolynomialExtensions.Parse(x1), PolynomialExtensions.Parse(y1)) : 
            Point<Polynomial>.Infinity;
            var result = resX != null && resY != null ? new Point<Polynomial>(PolynomialExtensions.Parse(resX), PolynomialExtensions.Parse(resY)) : 
            Point<Polynomial>.Infinity;
            Assert.AreEqual(expected, IoHelper.FormatMul(left, k.ToString(), result, PolynomialExtensions.ToNumberString));
        }
        
        [Test]
        [TestCase("a (1, 2) (3, 4)", new []{"(1, 2) (3, 4)", "1", "2", "3", "4"})]
        [TestCase("a (1, 2) (1, 2)", new []{"(1, 2) (1, 2)", "1", "2", "1", "2"})]
        [TestCase("a (, ) (, )", new []{"(, ) (, )", "", "", "", ""})]
        [TestCase("a (null, 2) (3, 4)", new []{"(null, 2) (3, 4)", "null", "2", "3", "4"})]
        [TestCase("a (null,2) (3 , 4)", new []{"(null,2) (3 , 4)", "null", "2", "3", "4"})]
        [TestCase("a (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9)", 
            new []{"(0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9)",
                "0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8", "0x289070fb05d38ff58321f2e800536d538ccdaa3d9",
                "0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8", "0x289070fb05d38ff58321f2e800536d538ccdaa3d9"})]
        public void GetAddParamsTest(string text, string[] expected)
        {
            Assert.AreEqual(expected.ToList(), IoHelper.GetAddParams(text));
        }
        
        [Test]
        [TestCase("m (1, 2) 123", new []{"(1, 2) 123", "1", "2", "123"})]
        [TestCase("m (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) 5846006549323611672814741753598448348329118574063", 
            new []{"(0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) 5846006549323611672814741753598448348329118574063",
                "0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8", "0x289070fb05d38ff58321f2e800536d538ccdaa3d9",
                "5846006549323611672814741753598448348329118574063"})]
        public void GetMulParamsTest(string text, string[] expected)
        {
            Assert.AreEqual(expected.ToList(), IoHelper.GetMulParams(text));
        }
    }
}