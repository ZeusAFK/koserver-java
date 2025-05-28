// Using Microsoft.Extensions.Configuration might be typical here
// For simplicity, a placeholder:
namespace KnightOnline.Infrastructure.Configuration
{
    public class ConfigLoader
    {
        public static AppSettings Load(string path = "appsettings.json")
        {
            // Placeholder - in a real app, use IConfiguration
            return new AppSettings
            {
                LoginServer = new ServerSettings { Port = 15100, ConnectionString = "your_login_db_connection" },
                GameServer = new ServerSettings { Port = 15001, ConnectionString = "your_game_db_connection" }
            };
        }
    }
}
