namespace KnightOnline.Domain.Networking
{
    public static class ClientOpcodes
    {
        public const ushort VersionRequest = 0x0001;
        public const ushort LoginRequest = 0x00F3;   // LS_LOGIN
        public const ushort EncryptionRequest = 0x00F2; // LS_CRYPTION
        public const ushort ServerListRequest = 0x00F5; // LS_SERVERLIST
        public const ushort NewsRequest = 0x00F6;       // LS_NEWS
    }

    public static class ServerOpcodes
    {
        public const ushort VersionResponse = 0x0002;
        public const ushort LoginResponse = 0x00F3;   // LS_LOGIN (used for response)
        public const ushort EncryptionResponse = 0x00F2; // LS_CRYPTION (used for response)
        public const ushort ServerListResponse = 0x00F5; // LS_SERVERLIST (used for response)
        public const ushort NewsResponse = 0x00F6;       // LS_NEWS (used for response)
    }
}
