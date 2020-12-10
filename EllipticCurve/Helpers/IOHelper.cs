using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using EllipticCurve.Models;

namespace EllipticCurve.Helpers
{
    public static class IoHelper
    {
        private const string AddPattern = "\\((.*), (.*)\\) \\((.*), (.*)\\)";
        private const string MulPattern = "\\((.*), (.*)\\) (.*)";

        public static void Print(string outputDir, string fileName, string text)
        {
            var path = $"{outputDir}\\{fileName}";

            using var sw = new StreamWriter(path);
            sw.WriteAsync(text);
        }
        public static string FormatAdd<T>(Point<T> left, Point<T> right, Point<T> result)
        {
            return $"{left} + {right} = {result}";
        }
        
        public static string FormatMul<T>(Point<T> left, BigInteger k, Point<T> result)
        {
            return $"{left} * {k} = {result}";
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
                .Select(x => ChangeNotation(x.Value))
                .ToList();
        }

        private static string ChangeNotation(string str)
        {
            return str.StartsWith("0x") ? BigInteger.Parse(str[Range.StartAt(2)], NumberStyles.HexNumber).ToString() : str;
        }
    }
}