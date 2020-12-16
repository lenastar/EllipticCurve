using EllipticCurve.Helpers;
using EllipticCurve.Managers;
using EllipticCurve.Models;
using ExtendedArithmetic;
using NUnit.Framework;
using Integer = System.Numerics.BigInteger;

namespace EllipticCurveTest
{
    public class GfNotSuperSingularManagerTest
    {
        [Test]
        [TestCase("x^3", "x", 0, null, null)]
        [TestCase("x^3", "x", 1, "x^3", "x")]
        [TestCase("x^3", "x", 2, "x^2 + x", "1 + x + x^2")]
        [TestCase("x^3", "x", 14, "x^2 + x", "1")]
        public void TestMul(string x, string y, int t, string resX, string resY)
        {
            var k = 4;
            var a = Polynomial.Parse("1");
            var b = Polynomial.Parse("1");
            var c = Polynomial.Parse("1");
            var xPol = Polynomial.Parse(x);
            var yPol = Polynomial.Parse(y);
            var left = new Point<Polynomial>(xPol, yPol);

            var manager = new GfCurveNotSuperSingularManager(k, a, b, c);
            var result = manager.Multiple(left, t);
            
            var resXPol = resX != null ? Polynomial.Parse(resX) : null;
            var resYPol = resX != null ? Polynomial.Parse(resY) : null;
            var expectedResult = new Point<Polynomial>(resXPol, resYPol);
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        [TestCase("x + x^2", "1", "x + x^2", "1", "1", "1 + x + x^2")]
        [TestCase("1", "x + x^2", "x + x^2", "1 + x + x^2", "1 + x + x^2", "x + x^2")]
        [TestCase("x^3", "x", "x^3", "x + x^3", null, null)]
        public void TestAdd(string x1, string y1, string x2, string y2, string resX, string resY)
        {
            var k = 4;
            var a = Polynomial.Parse("1");
            var b = Polynomial.Parse("1");
            var c = Polynomial.Parse("1");
            var x1Pol = Polynomial.Parse(x1);
            var y1Pol = Polynomial.Parse(y1);
            var x2Pol = Polynomial.Parse(x2);
            var y2Pol = Polynomial.Parse(y2);
            var left = new Point<Polynomial>(x1Pol, y1Pol);
            var right = new Point<Polynomial>(x2Pol, y2Pol);

            var manager = new GfCurveNotSuperSingularManager(k, a, b, c);
            var result = manager.Add(left, right);
            
            var resXPol = resX != null ? Polynomial.Parse(resX) : null;
            var resYPol = resX != null ? Polynomial.Parse(resY) : null;
            var expectedResult = new Point<Polynomial>(resXPol, resYPol);
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        [TestCase("4373527398576640063579304354969275615843559206632", "3705292482178961271312284701371585420180764402649", "2", "0xcb5ca2738fe300aacfb00b42a77b828d8a5c41eb", "0x229c79e9ab85f90acd3d5fa3a696664515efefa6b")]
        [TestCase("4373527398576640063579304354969275615843559206632", "3705292482178961271312284701371585420180764402649", 
        "5846006549323611672814741753598448348329118574063", null, null )]
        public void TestFipsMul(string x, string y, string t, string resX, string resY)
        {
            var k = 163;
            var a = Polynomial.Parse("1");
            var b = Polynomial.Parse("1");
            var c = Polynomial.Parse("1");
            var xPol = PolynomialExtensions.Parse(x);
            var yPol = PolynomialExtensions.Parse(y);
            var left = new Point<Polynomial>(xPol, yPol);

            var m = Integer.Parse(t);
            var manager = new GfCurveNotSuperSingularManager(k, a, b, c);
            var result = manager.Multiple(left, m);
            
            var resXPol = resX != null ? PolynomialExtensions.Parse(resX) : null;
            var resYPol = resY != null ? PolynomialExtensions.Parse(resY) : null;
            var expectedResult = new Point<Polynomial>(resXPol, resYPol);
            
            Assert.AreEqual(expectedResult, result);
        }
    }
}