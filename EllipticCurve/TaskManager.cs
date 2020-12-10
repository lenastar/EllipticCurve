using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using EllipticCurve.Helpers;
using EllipticCurve.Interfaces;
using EllipticCurve.Managers;
using EllipticCurve.Models;
using ExtendedArithmetic;


namespace EllipticCurve
{
    public static class TaskManager
    {
        public static void RunFileTasks(string fileName, string inputDir, string outputDir)
        {
            var text = File.ReadAllLines(fileName);
            var shortFileName = fileName.Remove(0, inputDir.Length);
            var type = text[0];

            switch (type)
            {
                case "Z_P":
                {
                    var p = BigInteger.Parse(text[1]);
                    var a = BigInteger.Parse(text[2]);
                    var b = BigInteger.Parse(text[3]);
                    var manager = new ZpCurveOperationManager(p, a, b);

                    var result = ExecuteTasks(text.Skip(4), manager, s => BigInteger.Parse(s));

                    IoHelper.Print(outputDir, shortFileName, result);

                    break;
                }
                case "GF(2^m)":
                {
                    var order = long.Parse(text[1]);
                    var a1 = Polynomial.Parse(text[2]);
                    var a2 = Polynomial.Parse(text[3]);
                    var a3 = Polynomial.Parse(text[4]);
                    var a4 = Polynomial.Parse(text[5]);
                    var a5 = Polynomial.Parse(text[6]);

                    ICurveOperationManager<Polynomial> manager;

                    if (CheckSuperSingular(a1, a2, a3, a4, a5))
                    {
                        manager = new GfCurveSuperSingularManager(order, a2, a4, a5);
                    }
                    else if (CheckNotSuperSingular(a1, a2, a3, a4, a5))
                    {
                        manager = new GfCurveNotSuperSingularManager(order, a1, a3, a5);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            $"Кривая должна быть суперсингулярной или несуперсингулярной. Файл: {fileName}");
                    }

                    var result = ExecuteTasks(text.Skip(7), manager, Polynomial.Parse);

                    IoHelper.Print(outputDir, shortFileName, result);

                    break;
                }
                default:
                    throw new NotSupportedException($"Неизвестный тип: {type}. Файл: {fileName}");
            }
        }

        private static bool CheckSuperSingular(Polynomial a1, Polynomial a2, Polynomial a3, Polynomial a4,
            Polynomial a5)
        {
            var zero = Polynomial.Zero;
            return a1.Equals(zero) && !a2.Equals(zero) && a3.Equals(zero) &&
                   !a4.Equals(zero) && !a5.Equals(zero);
        }

        private static bool CheckNotSuperSingular(Polynomial a1, Polynomial a2, Polynomial a3, Polynomial a4,
            Polynomial a5)
        {
            var zero = Polynomial.Zero;
            return !a1.Equals(zero) && a2.Equals(zero) && !a3.Equals(zero) &&
                   a4.Equals(zero) && !a5.Equals(zero);
        }

        public static string ExecuteTasks<T>(IEnumerable<string> tasks, ICurveOperationManager<T> manager, Func<string, T> expression)
        {
            var outputs = new StringBuilder();
            foreach (var task in tasks)
            {
                if (task.StartsWith('a'))
                {
                    var @params = IoHelper.GetAddParams(task);

                    var left = GetPoint(@params[1], @params[2], expression);
                    var right = GetPoint(@params[3], @params[4], expression);
                    var result = manager.Add(left, right);

                    outputs.AppendLine(IoHelper.FormatAdd(left, right, result));
                }
                else if (task.StartsWith('m'))
                {
                    var @params = IoHelper.GetMulParams(task);

                    var left = GetPoint(@params[1], @params[2], expression);
                    var right = BigInteger.Parse(@params[3]);
                    var result = manager.Multiple(left, right);

                    outputs.AppendLine(IoHelper.FormatMul(left, right, result));
                }
                else
                {
                    Console.WriteLine($"Wrong task: {task}");
                }
            }

            return outputs.ToString();
        }

        private static Point<T> GetPoint<T>(string x1, string y1, Func<string, T> expression)
        {
            if (x1.Length != 0 && y1.Length != 0)
            {
                return new Point<T>(expression(x1), expression(y1));
            }

            if (x1.Length != 0)
            {
                return new Point<T>(expression(x1), default);
            }

            if (y1.Length != 0)
            {
                return new Point<T>(default, expression(y1));
            }

            return Point<T>.Infinity;
        }
    }
}