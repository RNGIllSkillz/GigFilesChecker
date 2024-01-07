using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GigFilesChecker.Setup.Config
{
    internal class Downloader
    {
        public static void Download(string GigPath)
        {
            string releaseUrl = "https://api.github.com/repos/BigBoot/GiganticEmu/releases/latest";
            string response = SendRequest(releaseUrl);
            int curlExitCode = response != null ? 0 : -1;

            if (curlExitCode == 0)
            {
                JArray assets = GetJsonValue(response, "assets") as JArray;

                // Define target directories for each file
                string[] fileDirectories =
                {
                    GigPath + "/Binaries/Win64",   // Directory for ArcSDK.dll
                    GigPath   // Directory for GiganticEmu.Agent
                };

                // Search for the specific files
                string[] fileNames = { "ArcSDK.dll", "GiganticEmu.Agent" };
                string[] downloadUrls = new string[fileNames.Length];
                int i = 0;
                foreach (string fileName in fileNames)
                {
                    if (assets.FirstOrDefault(a => (string)a["name"] == fileName) is JObject asset)
                    {
                        string url = asset.Value<string>("browser_download_url");
                        downloadUrls[i] = url;

                        // Set target directory for the file
                        string directory = fileDirectories[i];
                        i++;

                        // Download the file
                        Console.WriteLine($"Downloading file: {fileName}");
                        bool downloadSuccess = DownloadFile($"{directory}/{fileName}", url);

                        // Check if download was successful
                        if (downloadSuccess)
                        {
                            Console.WriteLine($"Downloaded {fileName} successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to download {fileName}. Curl exit code: {curlExitCode}");
                            Environment.Exit(1);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Failed to fetch release data. Curl exit code: {curlExitCode}");
            }
        }

        private static string SendRequest(string url)
        {
            string response = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    response = client.DownloadString(url);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending request: {e.Message}");
            }
            return response;
        }

        private static JToken GetJsonValue(string json, string key)
        {
            JToken parsedJson = JToken.Parse(json);
            JToken value = parsedJson[key];
            return value;
        }

        private static bool DownloadFile(string filePath, string url)
        {
            bool success = false;
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, filePath);
                    success = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error downloading file: {e.Message}");
            }
            return success;
        }
    }
}
