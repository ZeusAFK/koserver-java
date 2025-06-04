namespace KnightOnline.Infrastructure.Configuration.Models
{
    public class GameServerSettings
    {
        public NetworkSettings Network { get; set; } = new NetworkSettings();
        public string Version { get; set; } = "0";
        // We can add PlayerSpecific settings later if koserver.game.player.properties is complex
        public string ConnectionString { get; set; } = string.Empty;
    }
}
