using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic;
using GigFilesChecker.Setup.Dirs;
using GigFilesChecker.Setup.Config;
using GigFilesChecker.MODELS;

class Program
{
    private static readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim(false);
    static void Main()
    {        
        //Getting Environment variables
        string sourceDirectory = Environment.GetEnvironmentVariable("ENV_PATH_TO_HOST");
        string destinationDirectory = Environment.GetEnvironmentVariable("ENV_PATH_TO_VOL");
        string MainApp = Environment.GetEnvironmentVariable("ENV_APP_EXE_NAME");
        if (sourceDirectory == null)
        {
            Console.WriteLine("ENV_PATH_TO_HOST cannot be NULL");
            return;
        }
        if (destinationDirectory == null)
        {
            Console.WriteLine("ENV_PATH_TO_VOL cannot be NULL");
            return;
        }
        if (MainApp == null)
        {
            Console.WriteLine("ENV_APP_EXE_NAME cannot be NULL");
            return;
        }

        //Copy files from host to Docker Volume
        Console.WriteLine("Checking for files presence");
        DirectoryComparer.CopyMissingFiles(sourceDirectory, destinationDirectory); 
        Console.WriteLine();

        //Ckeck hashes and reaplace files if needed
        Console.WriteLine("Checking files integrity");
        HashChecker.CheckAndCopyFiles(sourceDirectory, destinationDirectory);
        Console.WriteLine();

        //Download latest server assets
        Console.WriteLine("Downloading assets");
        Downloader.Download(destinationDirectory);
        Console.WriteLine();

        //Creating server config
        try
        {
            Configure.CreateJsonFile(new ServerConfig()
            {
                HttpPort = int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT")),
                ServerPort = int.Parse(Environment.GetEnvironmentVariable("SERVER_PORT")),
                ServerUrl = Environment.GetEnvironmentVariable("SERVER_URL"),
                MaxInstances = int.Parse(Environment.GetEnvironmentVariable("MAX_INSTANCES")),
                Title = Environment.GetEnvironmentVariable("TITLE"),
                GiganticPath = Environment.GetEnvironmentVariable("GIGANTIC_PATH"),
                ApiKey = Environment.GetEnvironmentVariable("API_KEY")
            }, destinationDirectory);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return;
        }

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
