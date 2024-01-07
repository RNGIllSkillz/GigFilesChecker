using System;
using System.IO;
using Newtonsoft.Json;
using GigFilesChecker.MODELS;

namespace GigFilesChecker.Setup.Config
{
    internal class Configure
    {
        public static void CreateJsonFile(ServerConfig Config, string GamePath)
        {
            var jsonData = new
            {
                http_port = Config.HttpPort,
                server_url = Config.ServerUrl,
                server_port = Config.ServerPort,
                max_instances = Config.MaxInstances,
                title = Config.Title,
                gigantic_path = Config.GiganticPath,
                api_key = Config.ApiKey
            };

            var json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            File.WriteAllText(Path.Combine(GamePath, "config.json"), json);
        }
    }
}
