using GigFilesChecker.HashCalculatorNs;
using System;
using System.Collections.Generic;
using System.IO;

namespace GigFilesChecker.HashCheckerNs
{
    public static class HashChecker
    {
        private const string _hashFileName = "hashes.txt"; // File name to store the hashes

        public static void CheckAndCopyFiles(string sourceDirectory, string destinationDirectory)
        {
            var firstHashes = ReadHashesFromFile(destinationDirectory);
            if (firstHashes == null)
            {
                Console.WriteLine("Hash file not found. Creating new hash file...");
                firstHashes = CalculateAndSaveHashes(destinationDirectory);
            }

            var secondHashes = CalculateAndSaveHashes(sourceDirectory);

            // Compare hashes and copy files if necessary
            foreach (var secondHash in secondHashes)
            {
                if (!firstHashes.TryGetValue(secondHash.Key, out string? firstHash) || firstHash != secondHash.Value)
                {
                    string destinationPath = Path.Combine(destinationDirectory, secondHash.Key[(sourceDirectory.Length + 1)..]);
                    Console.WriteLine($"Copying file: {secondHash.Key} to {destinationPath}");                    
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    File.Copy(secondHash.Key, destinationPath, true);                    
                }
            }
            Console.WriteLine("File checking and copying complete.");
        }

        private static Dictionary<string, string>? ReadHashesFromFile(string directory)
        {
            string hashFilePath = Path.Combine(directory, _hashFileName);
            if (File.Exists(hashFilePath))
            {
                var fileHashes = new Dictionary<string, string>();
                var lines = File.ReadAllLines(hashFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        fileHashes.Add(parts[0], parts[1]);
                    }
                }
                return fileHashes;
            }
            return null;
        }

        private static Dictionary<string, string> CalculateAndSaveHashes(string directory)
        {
            var fileHashes = HashCalculator.CalculateCRC32Hashes(directory);
            SaveHashesToFile(directory, fileHashes);
            return fileHashes;
        }

        private static void SaveHashesToFile(string directory, Dictionary<string, string> fileHashes)
        {
            string hashFilePath = Path.Combine(directory, _hashFileName);
            using var writer = new StreamWriter(hashFilePath);
            foreach (var fileHash in fileHashes)
            {
                writer.WriteLine($"{fileHash.Key}|{fileHash.Value}");
            }
        }
    }
}
