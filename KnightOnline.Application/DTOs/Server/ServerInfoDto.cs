namespace KnightOnline.Application.DTOs.Server
{
    public class ServerInfoDto
    {
        public int ServerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public int UserMax { get; set; }
        public int UserMaxFree { get; set; } // Represents the count of users under the 'free user' limit if applicable
        public int Category { get; set; } // Could represent server type, e.g., 0=Normal, 1=Premium, etc.
        public int Port { get; set; } // Adding Port as it's essential for client connection
    }
}
