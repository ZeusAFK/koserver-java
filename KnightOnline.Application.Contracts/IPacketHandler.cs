using System.Threading.Tasks;
using KnightOnline.Domain.Networking; // For Packet

namespace KnightOnline.Application.Contracts.Networking
{
    // TPacket could be constrained further if specific packet types are defined
    // public interface IPacketHandler<in TPacket> where TPacket : Packet
    public interface IPacketHandler
    {
        ushort Opcode { get; } // Each handler declares which opcode it handles
        Task HandlePacketAsync(IClientSession session, Packet packet);
    }
}
