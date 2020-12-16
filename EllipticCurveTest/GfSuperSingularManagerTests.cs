using EllipticCurve.Managers;
using EllipticCurve.Models;
using ExtendedArithmetic;
using NUnit.Framework;

namespace EllipticCurveTest
{
    public class GfSuperSingularManagerTest
    {
        [Test]
        [TestCase("x^3", "x", 0, null, null)]
        [TestCase("x^3", "x", 1, "x^3", "x")]
        [TestCase("x + x^2", "1", 2, "1 + x + x^2", "x + x^2")]
        public void TestMul(string x, string y, int t, string resX, string resY)
        {
            var k = 4;
            var a = Polynomial.Parse("1");
            var b = Polynomial.Parse("1");
            var c = Polynomial.Parse("1");
            var xPol = Polynomial.Parse(x);
            var yPol = Polynomial.Parse(y);
            var left = new Point<Polynomial>(xPol, yPol);

            var manager = new GfCurveSuperSingularManager(k, a, b, c);
            var result = manager.Multiple(left, t);
            
            var resXPol = resX != null ? Polynomial.Parse(resX) : null;
            var resYPol = resX != null ? Polynomial.Parse(resY) : null;
            var expectedResult = new Point<Polynomial>(resXPol, resYPol);
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        [TestCase("x + x^2", "1", "x + x^2", "1", "1 + x + x^2", "x + x^2")]
        [TestCase("1", "x + x^2", "x + x^2", "1 + x + x^2", "0", "1")]
        [TestCase("x + x^2", "x + x^2", "x + x^2", "1 + x + x^2", null, null)]
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

            var manager = new GfCurveSuperSingularManager(k, a, b, c);
            var result = manager.Add(left, right);
            
            var resXPol = resX != null ? Polynomial.Parse(resX): null;
            var resYPol = resY != null ?Polynomial.Parse(resY) : null;
            var expectedResult = new Point<Polynomial>(resXPol, resYPol);
            Assert.AreEqual(expectedResult, result);
        }
    }
}