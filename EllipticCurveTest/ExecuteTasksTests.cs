using System.Text;
using EllipticCurve;
using EllipticCurve.Helpers;
using EllipticCurve.Managers;
using ExtendedArithmetic;
using NUnit.Framework;
using Integer = System.Numerics.BigInteger;

namespace EllipticCurveTest
{
    public class ExecuteTasksTests
    {
        [Test]
        public void ExecuteGfNotSuperSingularTasksTest()
        {
            var tasks = new[]
            {
                "m (8, 2) 0",
                "m (8, 2) 1",
                "m (8, 2) 2",
                "m (8, 2) 14",
                "a (6, 1) (6, 1)",
                "a (1, 6) (6,7)",
                "a (8, 2) (8,10)",
            };

            var manager = new GfCurveNotSuperSingularManager(4, Polynomial.One, Polynomial.One, Polynomial.One);
            var result = TaskManager.ExecuteTasks(tasks, manager, PolynomialExtensions.Parse, PolynomialExtensions.ToNumberString);

            var expectedResult = new StringBuilder()
                .AppendLine("(8, 2) * 0 = (, )")
                .AppendLine("(8, 2) * 1 = (8, 2)")
                .AppendLine("(8, 2) * 2 = (6, 7)")
                .AppendLine("(8, 2) * 14 = (6, 1)")
                .AppendLine("(6, 1) + (6, 1) = (1, 7)")
                .AppendLine("(1, 6) + (6, 7) = (7, 6)")
                .AppendLine("(8, 2) + (8, 10) = (, )")
                .ToString();
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void ExecuteGfNotSuperSingularNistTasksTest()
        {
            var tasks = new[]
            {
                "m (0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8,0x289070fb05d38ff58321f2e800536d538ccdaa3d9) 0x4000000000000000000020108a2e0cc0d99f8a5ef",
            };

            var manager = new GfCurveNotSuperSingularManager(163, Polynomial.One, Polynomial.One, Polynomial.One);
            var result = TaskManager.ExecuteTasks(tasks, manager, PolynomialExtensions.Parse, PolynomialExtensions.ToNumberString);

            var expectedResult = new StringBuilder()
                .AppendLine("(0x2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8, 0x289070fb05d38ff58321f2e800536d538ccdaa3d9) * 0x4000000000000000000020108a2e0cc0d99f8a5ef = (, )")
                .ToString();
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void ExecuteGfSuperSingularTasksTest()
        {
            var tasks = new[]
            {
                "m (8, 2) 0",
                "m (8, 2) 1",
                "m (6, 1) 2",
                "a (6, 1) (6, 1)",
                "a (1, 6) (6, 7)",
                "a (6, 6) (6, 7)"
            };

            var manager = new GfCurveSuperSingularManager(4, Polynomial.One, Polynomial.One, Polynomial.One);
            var result = TaskManager.ExecuteTasks(tasks, manager, PolynomialExtensions.Parse, PolynomialExtensions.ToNumberString);

            var expectedResult = new StringBuilder()
                .AppendLine("(8, 2) * 0 = (, )")
                .AppendLine("(8, 2) * 1 = (8, 2)")
                .AppendLine("(6, 1) * 2 = (7, 6)")
                .AppendLine("(6, 1) + (6, 1) = (7, 6)")
                .AppendLine("(1, 6) + (6, 7) = (0, 1)")
                .AppendLine("(6, 6) + (6, 7) = (, )")
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
            var result = TaskManager.ExecuteTasks(tasks, manager, s => Integer.Parse(s), IntegerExtensions.ToNumberString);

            var expectedResult = new StringBuilder()
                .AppendLine("(, ) * 1 = (, )")
                .AppendLine("(1, 2) * 1 = (1, 2)")
                .AppendLine("(1, 2) * 2 = (2, 2)")
                .AppendLine("(, ) + (1, 0) = (1, 0)")
                .AppendLine("(1, 2) + (1, -2) = (, )")
                .AppendLine("(1, 0) + (1, 0) = (, )")
                .AppendLine("(1, 2) + (2, 1) = (1, 1)")
                .AppendLine("(1, 2) + (1, 2) = (2, 2)")
                .ToString();
            Assert.AreEqual(expectedResult, result);
        }
    }
}