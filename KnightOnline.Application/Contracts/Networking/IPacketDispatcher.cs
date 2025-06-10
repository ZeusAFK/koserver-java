using System.Threading.Tasks;
using KnightOnline.Domain.Networking; // For Packet

namespace KnightOnline.Application.Contracts.Networking
{
    public interface IPacketDispatcher
    {
        Task DispatchPacketAsync(IClientSession session, Packet packet);
        void RegisterHandler(IPacketHandler handler);
        // void UnregisterHandler(ushort opcode); // Optional
    }
}
