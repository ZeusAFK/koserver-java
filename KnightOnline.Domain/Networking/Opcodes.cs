namespace KnightOnline.Domain.Networking
{
    public static class ClientOpcodes
    {
        public const ushort VersionRequest = 0x0001;
        public const ushort LoginRequest = 0x00F3; // LS_LOGIN
    }

    public static class ServerOpcodes
    {
        public const ushort VersionResponse = 0x0002;
        // LS_LOGIN (0x00F3) is used for login response as well,
        // but we can define a conceptual name if needed, or just reuse.
        // For clarity in C# code that creates response packets, let's define it.
        public const ushort LoginResponse = 0x00F3;
    }
}
