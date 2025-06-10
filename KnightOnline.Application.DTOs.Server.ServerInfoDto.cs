namespace KnightOnline.Application.DTOs.Server
{
    public class ServerInfoDto
    {
        public string Ip { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public short UserCount { get; set; }
        public short ServerId { get; set; } // Was int in Java ServerEntity, but sent as short
        public short Category { get; set; } // Consider making this an enum later
        public short UserMax { get; set; }
        public short UserMaxFree { get; set; }
        // The constant zero byte and empty strings for KingName/Notices
        // will be handled directly during packet construction in the PacketHandler,
        // as they are fixed and not part of the core server data model.
    }
}
