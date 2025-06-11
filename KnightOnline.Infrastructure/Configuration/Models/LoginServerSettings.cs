using System.Collections.Generic; // Required for List

namespace KnightOnline.Infrastructure.Configuration.Models
{
    public class LoginServerSettings
    {
        public NetworkSettings Network { get; set; } = new NetworkSettings();
        public string Version { get; set; } = "0";
        public string FtpUrl { get; set; } = "127.0.0.1";
        public string FtpPath { get; set; } = "/";
        public bool AutoCreateAccount { get; set; } = false;
        public string ConnectionString { get; set; } = string.Empty;

        public List<ServerConfigItem> ServerList { get; set; } = new List<ServerConfigItem>();
    }
}
