using GigFilesChecker.DirectoryComparerNs;
using GigFilesChecker.HashCheckerNs;
using System;
using System.IO;

class Program
{
    private const string configFileName = "directories.txt";
    static void Main()
    {
        string? programDirectory = AppDomain.CurrentDomain.BaseDirectory;
        if (programDirectory == null)
        {
            Console.WriteLine("Error! programDirectory cannot be null");
            return;
        }
        string configFilePath = Path.Combine(programDirectory, configFileName);

        Console.WriteLine($"Reading conf file: {configFilePath}");
        var directories = ReadDirectoriesFromFile(configFilePath);
        Console.WriteLine();
        if (directories == null)
        {
            Console.WriteLine("Directory configuration file not found. Please create a file named 'directories.txt' and add the source and destination directories.");
            return;
        }
        string sourceDirectory = directories.Item1;
        string destinationDirectory = directories.Item2;

        CheckAndCreateDirectory(destinationDirectory);

        Console.WriteLine("Checking for files presence");
        DirectoryComparer.CopyMissingFiles(sourceDirectory, destinationDirectory); 
        Console.WriteLine();

        Console.WriteLine("Checking files integrity");
        HashChecker.CheckAndCopyFiles(sourceDirectory, destinationDirectory);
        Console.WriteLine();
    }
    private static Tuple<string, string>? ReadDirectoriesFromFile(string filePath)
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
