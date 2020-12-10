using System.Numerics;
using EllipticCurve.Managers;
using EllipticCurve.Models;
using NUnit.Framework;

namespace EllipticCurveTest
{
    public class ZpCurveManagerTests
    {
        [Test]
        [TestCase(null, null, 1L, 0L, 1L, 0L)]
        [TestCase(1L, 2L, 1L, -2L, null, null)]
        [TestCase(1L, 0L, 1L, 0L, null, null)]
        [TestCase(1L, 2L, 2L, 1L, -2, -2)]
        [TestCase(1L, 2L, 1L, 2L, 2, -1)]
        public void TestAdd(long? x1, long? y1, long? x2, long? y2, long? x, long? y)
        {
            var manager = new ZpCurveOperationManager(3, -1, 3);
            var left = new Point<BigInteger?>(x1, y1);
            var right = new Point<BigInteger?>(x2, y2);
            var expectedResult = new Point<BigInteger?>(x, y);

            var result = manager.Add(left, right);
            
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        [TestCase(null, null, 1, null, null)]
        [TestCase(1L, 2L, 1, 1L, 2L)]
        [TestCase(1L, 2L, 2, 2, -1)]
        public void TestMul(long? x1, long? y1, int k, long? x, long? y)
        {
            var manager = new ZpCurveOperationManager(3, -1, 3);
            var left = new Point<BigInteger?>(x1, y1);
            var expectedResult = new Point<BigInteger?>(x, y);

            var result = manager.Multiple(left, k);
            
            Assert.AreEqual(expectedResult, result);
        }
    }
}