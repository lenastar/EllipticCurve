using System;
using System.Diagnostics;
using System.IO;
using System.Threading;


namespace EllipticCurve
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var inputDir = args[0];
            var inputFileNames = Directory.GetFiles(inputDir);
            var outputDir = args[1];

            foreach (var fileName in inputFileNames)
            { 
                ThreadPool.QueueUserWorkItem(x => TaskManager.RunFileTasks(fileName, inputDir, outputDir));
            }

            Console.WriteLine($"Результаты находятся в папке: {outputDir}");
        }
    }
}
