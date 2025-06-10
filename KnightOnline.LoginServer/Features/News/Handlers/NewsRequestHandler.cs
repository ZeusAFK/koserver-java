using System;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Domain.Networking;
using KnightOnline.Application.Features.News.Queries; // For GetNewsQuery
using MediatR;
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.LoginServer.Features.News.Handlers
{
    public class NewsRequestHandler : IPacketHandler
    {
        private readonly IMediator _mediator;
        // private readonly ILogger<NewsRequestHandler> _logger; // Optional

        public ushort Opcode => ClientOpcodes.NewsRequest; // 0x00F6

        // public NewsRequestHandler(IMediator mediator, ILogger<NewsRequestHandler> logger)
        public NewsRequestHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            // _logger = logger;
        }

        public async Task HandlePacketAsync(IClientSession session, Packet requestPacket)
        {
            // _logger?.LogInformation($"Session {session.SessionId}: Received NewsRequest (Opcode: {requestPacket.Opcode}).");
            Console.WriteLine($"Session {session.SessionId}: Received NewsRequest (Opcode: {requestPacket.Opcode})."); // Placeholder

            // Request packet for LS_NEWS is expected to have an empty payload.
            // No data needs to be read from requestPacket.

            var getNewsQuery = new GetNewsQuery();
            GetNewsQuery.Response newsResponseData = await _mediator.Send(getNewsQuery);

            var responsePacket = new Packet(ServerOpcodes.NewsResponse); // Opcode 0x00F6

            responsePacket.WriteString(newsResponseData.Title);   // First string: "LoginNotice"
            responsePacket.WriteString(newsResponseData.Content); // Second string: "<empty>"

            // _logger?.LogInformation($"Session {session.SessionId}: Sending NewsResponse.");
            Console.WriteLine($"Session {session.SessionId}: Sending NewsResponse."); // Placeholder
            await session.SendPacketAsync(responsePacket);
        }
    }
}
