using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using EllipticCurve.Enums;
using EllipticCurve.Models;
using Integer = System.Numerics.BigInteger;

namespace EllipticCurve.Helpers
{
    public static class IoHelper
    {
        private const string AddPattern = "\\((.*)\\s*,\\s*(.*)\\) \\((.*)\\s*,\\s*(.*)\\)";
        private const string MulPattern = "\\((.*)\\s*,\\s*(.*)\\)\\s*(.*)";

        public static void Print(string outputDir, string fileName, string text)
        {
            var path = $"{outputDir}\\{fileName}";

            using var sw = new StreamWriter(path);
            sw.Write(text);
        }
        public static string FormatAdd<T>(Point<T> left, Point<T> right, Point<T> result, Func<T, NumericSystem, string> toString)
        {
            var leftF = $"({toString(left.X, left.NumericSystemX)}, {toString(left.Y, left.NumericSystemY)})";
            var rightF = $"({toString(right.X, right.NumericSystemX)}, {toString(right.Y, right.NumericSystemY)})";
            var resultF = $"({toString(result.X, left.NumericSystemX & right.NumericSystemX)}, {toString(result.Y, left.NumericSystemY & right.NumericSystemY)})";
            
            return $"{leftF} + {rightF} = {resultF}";
        }
        
        public static string FormatMul<T>(Point<T> left, string k, Point<T> result, Func<T, NumericSystem, string> 
        toString)
        {
            var leftF = $"({toString(left.X, left.NumericSystemX)}, {toString(left.Y, left.NumericSystemY)})";
            var resultF = $"({toString(result.X, left.NumericSystemX)}, {toString(result.Y, left.NumericSystemY)})";

            return $"{leftF} * {k} = {resultF}";
        }

        public static List<string> GetAddParams(string text)
        {
           var @params = GetParams(text, AddPattern);
            
            if (@params.Count != 5)
            {
                throw new ArgumentException($"Некорректный формат ввода для сложения в строке: {text}. ");
            }

            return @params;
        }
        
        public static List<string> GetMulParams(string text)
        {
            var @params = GetParams(text, MulPattern);

            if (@params.Count != 4)
            {
                throw new ArgumentException($"Некорректный формат ввода для умножения в строке: {text}. ");
            }

            return @params;
        }

        private static List<string> GetParams(string text, string pattern)
        {
            var t = new Regex(pattern);
            var matched = t.Match(text);

            if (!matched.Success || matched.Length == 0)
            {
                throw new ArgumentException($"Некорректный формат ввода в строке: {text}.");
            }

            return matched
                .Groups
                .Values
                .Select(x => x.Value.Trim())
                .ToList();
        }

        public static (string Changed, NumericSystem BaseSystem) ChangeNotationToDecimalWithBaseSystem(string str)
        {
            if (str.StartsWith("0x"))
            {
                return (BigInteger.Parse("0" + str[Range.StartAt(2)], NumberStyles.HexNumber).ToString(), NumericSystem.Hex);
            }

            return (str, NumericSystem.Decimal);
        }
        
        public static string ChangeNotationToDecimal(string str)
        {
            var (changed, _) = ChangeNotationToDecimalWithBaseSystem(str);
            return changed;
        }
    }
}