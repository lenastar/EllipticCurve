using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EllipticCurve.Enums;
using EllipticCurve.Helpers;
using EllipticCurve.Interfaces;
using EllipticCurve.Managers;
using EllipticCurve.Models;
using ExtendedArithmetic;
using Integer = System.Numerics.BigInteger;

namespace EllipticCurve
{
    public static class TaskManager
    {
        private const short GfParamsCount = 7;
        private const short ZpParamsCount = 4;
        public static void RunFileTasks(string fileName, string inputDir, string outputDir)
        {
            var text = File.ReadAllLines(fileName);
            var shortFileName = fileName.Remove(0, inputDir.Length);
            var type = text[0];

            switch (type)
            {
                case "Z_p":
                {
                    var changedToDecimal = text
                        .Take(ZpParamsCount)
                        .Select(IoHelper.ChangeNotationToDecimal)
                        .ToArray();
                    var p = Integer.Parse(changedToDecimal[1]);
                    var a = Integer.Parse(changedToDecimal[2]);
                    var b = Integer.Parse(changedToDecimal[3]);
                    var manager = new ZpCurveOperationManager(p, a, b);

                    var result = ExecuteTasks(text.Skip(ZpParamsCount), manager, s => Integer.Parse(s), 
                        IntegerExtensions.ToNumberString);

                    IoHelper.Print(outputDir, shortFileName, result);

                    break;
                }
                case "GF(2^m)":
                {
                    var changedToDecimal = text
                        .Take(GfParamsCount)
                        .Select(IoHelper.ChangeNotationToDecimal)
                        .ToArray();
                    var order = long.Parse(changedToDecimal[1]);
                    var a1 = Polynomial.Parse(changedToDecimal[2]);
                    var a2 = Polynomial.Parse(changedToDecimal[3]);
                    var a3 = Polynomial.Parse(changedToDecimal[4]);
                    var a4 = Polynomial.Parse(changedToDecimal[5]);
                    var a5 = Polynomial.Parse(changedToDecimal[6]);

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

                    var result = ExecuteTasks(text.Skip(GfParamsCount), manager, PolynomialExtensions.Parse, PolynomialExtensions.ToNumberString);

                    IoHelper.Print(outputDir, shortFileName, result);

                    break;
                }
                default:
                    throw new NotSupportedException($"Неизвестный тип: {type}. Файл: {fileName}");
            }
            
            Console.WriteLine($"Файл с результатами {fileName} записан в {outputDir}");
        }

        private static bool CheckSuperSingular(Polynomial a1, Polynomial a2, Polynomial a3, Polynomial a4, Polynomial a5)
        {
            var zero = Polynomial.Zero;
            return a1.Equals(zero) && !a2.Equals(zero) && a3.Equals(zero) &&
                   !a4.Equals(zero) && !a5.Equals(zero);
        }

        private static bool CheckNotSuperSingular(Polynomial a1, Polynomial a2, Polynomial a3, Polynomial a4, Polynomial a5)
        {
            var zero = Polynomial.Zero;
            return !a1.Equals(zero) && a2.Equals(zero) && !a3.Equals(zero) &&
                   a4.Equals(zero) && !a5.Equals(zero);
        }

        public static string ExecuteTasks<T>(IEnumerable<string> tasks, ICurveOperationManager<T> manager, Func<string, T> createInstance, Func<T, 
        NumericSystem, string> toStringFunc)
        {
            var outputs = new StringBuilder();
            foreach (var task in tasks)
            {
                if (task.StartsWith('a'))
                {
                    var @params = IoHelper.GetAddParams(task);

                    var left = GetPoint(@params[1], @params[2], createInstance);
                    var right = GetPoint(@params[3], @params[4], createInstance);
                    var result = manager.Add(left, right);

                    outputs.AppendLine(IoHelper.FormatAdd(left, right, result, toStringFunc));
                }
                else if (task.StartsWith('m'))
                {
                    var @params = IoHelper.GetMulParams(task);

                    var left = GetPoint(@params[1], @params[2], createInstance);
                    var right = Integer.Parse(IoHelper.ChangeNotationToDecimal(@params[3]));
                    var result = manager.Multiple(left, right);

                    outputs.AppendLine(IoHelper.FormatMul(left, @params[3], result, toStringFunc));
                }
                else
                {
                    Console.WriteLine($"Wrong task: {task}");
                }
            }

            return outputs.ToString();
        }

        private static Point<T> GetPoint<T>(string x1, string y1, Func<string, T> createInstance)
        {
            var (x, systemX) = IoHelper.ChangeNotationToDecimalWithBaseSystem(x1);
            var (y, systemY) = IoHelper.ChangeNotationToDecimalWithBaseSystem(y1);
            if (x1.Length != 0 && y1.Length != 0)
            {
                return new Point<T>(createInstance(x), createInstance(y), systemX, systemY);
            }

            if (x1.Length != 0)
            {
                return new Point<T>(createInstance(x1), default, systemX, systemY);
            }

            if (y1.Length != 0)
            {
                return new Point<T>(default, createInstance(y1), systemX, systemY);
            }

            return Point<T>.Infinity;
        }
    }
}