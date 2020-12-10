using EllipticCurve.Helpers;
using ExtendedArithmetic;
using NUnit.Framework;

namespace EllipticCurveTest
{
    public class GfFieldHelperTests
    {
        [Test]
        [TestCase("x^3", "x^3 + x^2 + x + 1")]
        [TestCase("x^3 + x^2 + x + 1", "x^3")]
        public void TestInvert(string pol, string inverted)
        {
            var k = Polynomial.Parse("x^4 + x + 1");
            var manager = new GfFieldHelper(k);
            var polynomial = Polynomial.Parse(pol);
            var expectedResult = Polynomial.Parse(inverted);

            var result = manager.Invert(polynomial);
            
            Assert.AreEqual(expectedResult, result);
        }
    }
}