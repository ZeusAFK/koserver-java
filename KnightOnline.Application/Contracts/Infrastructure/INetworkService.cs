namespace KnightOnline.Application.Contracts.Infrastructure
{
    public interface INetworkService
    {
        Task StartServerAsync(int port);
        Task StopServerAsync();
        // Other methods like SendPacketAsync, BroadcastPacketAsync etc.
    }
}
