using System.Numerics;
using System.Text;
using EllipticCurve;
using EllipticCurve.Managers;
using ExtendedArithmetic;
using NUnit.Framework;

namespace EllipticCurveTest
{
    public class ExecuteTasksTests
    {
        [Test]
        public void ExecuteGfNotSuperSingularTasksTest()
        {
            var tasks = new[]
            {
                "m (x^3, x) 0",
                "m (x^3, x) 1",
                "m (x^3, x) 2",
                "m (x^3, x) 14",
                "a (x + x^2, 1) (x + x^2, 1)",
                "a (1, x + x^2) (x + x^2, 1 + x + x^2)",
                "a (x^3, x) (x^3, x + x^3)"
            };

            var manager = new GfCurveNotSuperSingularManager(4, Polynomial.One, Polynomial.One, Polynomial.One);
            var result = TaskManager.ExecuteTasks(tasks, manager, Polynomial.Parse);

            var expectedResult = new StringBuilder()
                .AppendLine("(X^3, X) * 0 = (, )")
                .AppendLine("(X^3, X) * 1 = (X^3, X)")
                .AppendLine("(X^3, X) * 2 = (X^2 + X, X^2 + X + 1)")
                .AppendLine("(X^3, X) * 14 = (X^2 + X, 1)")
                .AppendLine("(X^2 + X, 1) + (X^2 + X, 1) = (1, X^2 + X + 1)")
                .AppendLine("(1, X^2 + X) + (X^2 + X, X^2 + X + 1) = (X^2 + X + 1, X^2 + X)")
                .AppendLine("(X^3, X) + (X^3, X^3 + X) = (, )")
                .ToString();
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void ExecuteGfSuperSingularTasksTest()
        {
            var tasks = new[]
            {
                "m (x^3, x) 0",
                "m (x^3, x) 1",
                "m (x + x^2, 1) 2",
                "a (x + x^2, 1) (x + x^2, 1)",
                "a (1, x + x^2) (x + x^2, 1 + x + x^2)",
                "a (x + x^2, x + x^2) (x + x^2, x + x^2 + 1)"
            };

            var manager = new GfCurveSuperSingularManager(4, Polynomial.One, Polynomial.One, Polynomial.One);
            var result = TaskManager.ExecuteTasks(tasks, manager, Polynomial.Parse);

            var expectedResult = new StringBuilder()
                .AppendLine("(X^3, X) * 0 = (, )")
                .AppendLine("(X^3, X) * 1 = (X^3, X)")
                .AppendLine("(X^2 + X, 1) * 2 = (0, X^2 + X + 1)")
                .AppendLine("(X^2 + X, 1) + (X^2 + X, 1) = (0, X^2 + X + 1)")
                .AppendLine("(1, X^2 + X) + (X^2 + X, X^2 + X + 1) = (X^2 + X + 1, 0)")
                .AppendLine("(X^2 + X, X^2 + X) + (X^2 + X, X^2 + X + 1) = (, )")
                .ToString();
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void ExecuteZpTasksTest()
        {
            var tasks = new[]
            {
                "m (, ) 1",
                "m (1, 2) 1",
                "m (1, 2) 2",
                "a (, ) (1, 0)",
                "a (1, 2) (1, -2)",
                "a (1, 0) (1, 0)",
                "a (1, 2) (2, 1)",
                "a (1, 2) (1, 2)"
            };

            var manager = new ZpCurveOperationManager(3, -1, 3);
            var result = TaskManager.ExecuteTasks(tasks, manager, s => BigInteger.Parse(s));

            var expectedResult = new StringBuilder()
                .AppendLine("(, ) * 1 = (, )")
                .AppendLine("(1, 2) * 1 = (1, 2)")
                .AppendLine("(1, 2) * 2 = (2, -1)")
                .AppendLine("(, ) + (1, 0) = (1, 0)")
                .AppendLine("(1, 2) + (1, -2) = (, )")
                .AppendLine("(1, 0) + (1, 0) = (, )")
                .AppendLine("(1, 2) + (2, 1) = (-2, -2)")
                .AppendLine("(1, 2) + (1, 2) = (2, -1)")
                .ToString();
            Assert.AreEqual(expectedResult, result);
        }
    }
}