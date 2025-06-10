namespace KnightOnline.Domain.Networking
{
    public static class ClientOpcodes
    {
        // Hypothetical opcode sent by the client to request version information
        public const ushort VersionRequest = 0x01;
        // Add other client-to-server opcodes here
    }

    public static class ServerOpcodes
    {
        // Hypothetical opcode sent by the server in response to a version request
        public const ushort VersionResponse = 0x02;
        // Add other server-to-client opcodes here
    }
}
