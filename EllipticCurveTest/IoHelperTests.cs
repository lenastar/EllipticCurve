﻿using System.Linq;
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
            var left = new Point<long?>(x1, y1);
            var right = new Point<long?>(x2, y2);
            var result = new Point<long?>(resX, resY);
            Assert.AreEqual(expected, IoHelper.FormatAdd(left, right, result));
        }
        
        [Test]
        [TestCase(1, 2, 3, 5, 6, "(1, 2) * 3 = (5, 6)" )]
        [TestCase(null, null, 3,  null, null, "(, ) * 3 = (, )" )]
        public void FormatMulLongTest(long? x1, long? y1, int k, long? resX, long? resY, string expected)
        {
            var left = new Point<long?>(x1, y1);
            var result = new Point<long?>(resX, resY);
            Assert.AreEqual(expected, IoHelper.FormatMul(left, k, result));
        }
        
        [Test]
        [TestCase("1", "2", "3", "4", "5", "6", "(1, 2) + (3, 4) = (5, 6)" )]
        [TestCase(null, null, "3", "4",  null, null, "(, ) + (3, 4) = (, )" )]
        [TestCase("x^1", "x^2", "x^3", "x^4", "x^5 + 1", "x^6 + x^2", "(X, X^2) + (X^3, X^4) = (X^5 + 1, X^6 + X^2)" )]
        public void FormatAddPolynomialTest(string x1, string y1, string x2, string y2, string resX, string resY, string expected)
        {
            var left = x1 != null && y1 != null ? new Point<Polynomial>(Polynomial.Parse(x1), Polynomial.Parse(y1)) : Point<Polynomial>.Infinity;
            var right = x2 != null && y2 != null ? new Point<Polynomial>(Polynomial.Parse(x2), Polynomial.Parse(y2)): Point<Polynomial>.Infinity;
            var result = resX != null && resY != null ? new Point<Polynomial>(Polynomial.Parse(resX), Polynomial.Parse(resY)) : Point<Polynomial>.Infinity;
            Assert.AreEqual(expected, IoHelper.FormatAdd(left, right, result));
        }
        
        [Test]
        [TestCase("1", "2", 3, "5", "6", "(1, 2) * 3 = (5, 6)" )]
        [TestCase(null, null, 3, null, null, "(, ) * 3 = (, )" )]
        [TestCase("x^1", "x^2", 4,  "x^5 + 1", "x^6 + x^2", "(X, X^2) * 4 = (X^5 + 1, X^6 + X^2)" )]
        public void FormatMulPolynomialTest(string x1, string y1, int k, string resX, string resY, string expected)
        {
            var left = x1 != null && y1 != null ? new Point<Polynomial>(Polynomial.Parse(x1), Polynomial.Parse(y1)) : Point<Polynomial>.Infinity;
            var result = resX != null && resY != null ? new Point<Polynomial>(Polynomial.Parse(resX), Polynomial.Parse(resY)) : Point<Polynomial>.Infinity;
            Assert.AreEqual(expected, IoHelper.FormatMul(left, k, result));
        }
        
        [Test]
        [TestCase("a (1, 2) (3, 4)", new []{"(1, 2) (3, 4)", "1", "2", "3", "4"})]
        [TestCase("a (1, 2) (1, 2)", new []{"(1, 2) (1, 2)", "1", "2", "1", "2"})]
        [TestCase("a (, ) (, )", new []{"(, ) (, )", "", "", "", ""})]
        [TestCase("a (null, 2) (3, 4)", new []{"(null, 2) (3, 4)", "null", "2", "3", "4"})]
        [TestCase("a (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9)", 
            new []{"(0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9)",
                "4373527398576640063579304354969275615843559206632", "3705292482178961271312284701371585420180764402649",
                "4373527398576640063579304354969275615843559206632", "3705292482178961271312284701371585420180764402649"})]
        public void GetAddParamsTest(string text, string[] expected)
        {
            Assert.AreEqual(expected.ToList(), IoHelper.GetAddParams(text));
        }
        
        [Test]
        [TestCase("m (1, 2) 123", new []{"(1, 2) 123", "1", "2", "123"})]
        [TestCase("m (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) 5846006549323611672814741753598448348329118574063", 
            new []{"(0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) 5846006549323611672814741753598448348329118574063",
                "4373527398576640063579304354969275615843559206632", "3705292482178961271312284701371585420180764402649",
                "5846006549323611672814741753598448348329118574063"})]
        public void GetMulParamsTest(string text, string[] expected)
        {
            Assert.AreEqual(expected.ToList(), IoHelper.GetMulParams(text));
        }
    }
}