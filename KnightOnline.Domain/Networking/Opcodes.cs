namespace KnightOnline.Domain.Networking
{
    public static class ClientOpcodes
    {
        public const ushort VersionRequest = 0x0001;
        public const ushort LoginRequest = 0x00F3;   // LS_LOGIN
        public const ushort EncryptionRequest = 0x00F2; // LS_CRYPTION
        // Add other client-to-server opcodes here
    }

    public static class ServerOpcodes
    {
        public const ushort VersionResponse = 0x0002;
        public const ushort LoginResponse = 0x00F3;   // LS_LOGIN (used for response)
        public const ushort EncryptionResponse = 0x00F2; // LS_CRYPTION (used for response)
        // Add other server-to-client opcodes here
    }
}
