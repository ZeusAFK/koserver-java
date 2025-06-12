using System.Net.Sockets;
using System.Threading.Tasks;
using KnightOnline.Domain.Networking; // For Packet
using System.Numerics; // For BigInteger

namespace KnightOnline.Application.Contracts.Networking
{
    public interface IClientSession
    {
        string SessionId { get; }
        Task SendPacketAsync(Packet packet);
        void Close();

        // Added for encryption
        void ActivateEncryption(BigInteger publicKey);
        bool IsEncrypted { get; }
    }
}
