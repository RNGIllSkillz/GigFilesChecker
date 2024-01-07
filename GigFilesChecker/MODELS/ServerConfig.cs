using System;
using System.Collections.Generic;
using System.Text;

namespace GigFilesChecker.MODELS
{
    internal class ServerConfig
    {
        public int HttpPort { get; set; }
        public int ServerPort { get; set; }
        public string ServerUrl { get; set; }        
        public int MaxInstances { get; set; }
        public string Title { get; set; }
        public string GiganticPath { get; set; }
        public string ApiKey { get; set; }
    }
}
