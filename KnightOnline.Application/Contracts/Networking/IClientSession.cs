using System.Net.Sockets;
using System.Threading.Tasks;
using KnightOnline.Domain.Networking; // For Packet

namespace KnightOnline.Application.Contracts.Networking
{
    public interface IClientSession
    {
        string SessionId { get; }
        Task SendPacketAsync(Packet packet);
        void Close();
        // Other properties like IPAddress, State, PlayerData etc. might be added later
    }
}
