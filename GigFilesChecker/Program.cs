using GigFilesChecker.DirectoryComparerNs;
using GigFilesChecker.HashCheckerNs;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Program
{
    private static ManualResetEventSlim _resetEvent = new ManualResetEventSlim(false);
    static void Main()
    {        
        //Getting Environment variables
        string sourceDirectory = Environment.GetEnvironmentVariable("ENV_PATH_TO_HOST");
        string destinationDirectory = Environment.GetEnvironmentVariable("ENV_PATH_TO_VOL");
        string MainApp = Environment.GetEnvironmentVariable("ENV_APP_EXE_NAME"); 

        //Copy files from host to Docker Volume
        Console.WriteLine("Checking for files presence");
        DirectoryComparer.CopyMissingFiles(sourceDirectory, destinationDirectory); 
        Console.WriteLine();

        //Ckeck hashes and reaplace files if needed
        Console.WriteLine("Checking files integrity");
        HashChecker.CheckAndCopyFiles(sourceDirectory, destinationDirectory);
        Console.WriteLine();

        //starting dotnetapp
        string AppPath = Path.Combine(destinationDirectory, MainApp);
        Process.Start(AppPath);
        _resetEvent.Wait();
    }
    private static Tuple<string, string> ReadDirectoriesFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 2)
            {
                return Tuple.Create(lines[0], lines[1]);
            }
        }
        return null;
    }
    private static void CheckAndCreateDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Console.WriteLine("Directory created: " + directoryPath);
        }
    }
}
