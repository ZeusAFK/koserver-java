using KnightOnline.Infrastructure.Configuration.Models;

namespace KnightOnline.Infrastructure.Configuration
{
    // Original ServerSettings class can be removed if no longer directly used,
    // or kept if it has other generic uses. For now, let's assume it's replaced.

    public class AppSettings
    {
        public LoginServerSettings LoginServer { get; set; } = new LoginServerSettings();
        public GameServerSettings GameServer { get; set; } = new GameServerSettings();

        // Example: Add a common logging configuration section
        // public LoggingSettings Logging { get; set; } = new LoggingSettings();
    }

    // Example: Placeholder for Logging settings, if you were to model log4j.properties
    // public class LoggingSettings
    // {
    //     public string LogLevel { get; set; } = "Information";
    //     // Add other properties as needed
    // }
}
