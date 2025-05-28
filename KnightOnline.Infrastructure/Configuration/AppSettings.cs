namespace KnightOnline.Infrastructure.Configuration
{
    public class ServerSettings
    {
        public int Port { get; set; }
        public string ConnectionString { get; set; }
    }

    public class AppSettings
    {
        public ServerSettings LoginServer { get; set; }
        public ServerSettings GameServer { get; set; }
    }
}
