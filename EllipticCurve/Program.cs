using System;
using System.IO;
using System.Linq;
using System.Threading;


namespace EllipticCurve
{
    public static class Program
    {
        static void Main()
        {
            var inputDir = "./inputs";
            var outputDir = "./outputs";

            var inputFileNames = Directory.GetFiles(inputDir);

            var threads = inputFileNames
                .Select(fileName => new Thread(x => TaskManager.RunFileTasks(fileName, inputDir, outputDir)))
                .ToArray();
            
            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
