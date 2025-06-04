namespace KnightOnline.Infrastructure.Configuration.Models
{
    public class NetworkSettings
    {
        public string BindHost { get; set; } = "*";
        public int BindPort { get; set; } = 0; // Default to 0, should be set
    }
}
