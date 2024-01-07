using System;
using System.IO;

namespace GigFilesChecker.Setup.Dirs
{
    public class DirectoryComparer
    {
        public static void CopyMissingFiles(string sourceDir, string destinationDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destinationDir))
            {
                Console.WriteLine("Source directory or destination directory does not exist.");
                return;
            }

            DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);
            DirectoryInfo destDirInfo = new DirectoryInfo(destinationDir);

            CopyFilesRecursive(sourceDirInfo, destDirInfo);
            Console.WriteLine("File copy completed.");
        }

        private static void CopyFilesRecursive(DirectoryInfo sourceDir, DirectoryInfo destDir)
        {
            foreach (FileInfo file in sourceDir.GetFiles())
            {
                string destFilePath = Path.Combine(destDir.FullName, file.Name);
                if (!File.Exists(destFilePath))
                {
                    Console.WriteLine($"Copying file: {file.FullName}");
                    file.CopyTo(destFilePath);
                }
            }

            foreach (DirectoryInfo subDir in sourceDir.GetDirectories())
            {
                string subDestDirPath = Path.Combine(destDir.FullName, subDir.Name);
                if (!Directory.Exists(subDestDirPath))
                {
                    Console.WriteLine($"Creating directory: {subDestDirPath}");
                    Directory.CreateDirectory(subDestDirPath);
                }

                CopyFilesRecursive(subDir, new DirectoryInfo(subDestDirPath));
            }
        }
    }
}
