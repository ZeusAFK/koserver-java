namespace KnightOnline.Infrastructure.Configuration.Models
{
    public class ServerConfigItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public int Port { get; set; }
        public int MaxUsers { get; set; } = 1000;
        public string? Category { get; set; } // e.g., "Normal", "Premium", or null
        public int UserCount { get; set; } = 0; // Default initial user count
        public int UserMaxFree {get; set; } = 800; // Default initial user max free
    }
}
