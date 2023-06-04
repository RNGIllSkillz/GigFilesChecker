using GigFilesChecker.CRC32Alg;
using System;
using System.Collections.Generic;
using System.IO;

namespace GigFilesChecker.HashCalculatorNs
{
    public static class HashCalculator
    {
        public static Dictionary<string, string> CalculateCRC32Hashes(string directoryPath)
        {
            var fileHashes = new Dictionary<string, string>();

            // Get all files in the directory and its subdirectories
            var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                Console.WriteLine("Calculating hash for:" + file);
                // Calculate the CRC32 hash for the file
                var crc32Hash = CalculateCRC32Hash(file);

                // Add the file path and its hash to the dictionary
                fileHashes.Add(file, crc32Hash);
            }
            return fileHashes;
        }

        private static string CalculateCRC32Hash(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                var crc32 = new CRC32();
                var hashBytes = crc32.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
