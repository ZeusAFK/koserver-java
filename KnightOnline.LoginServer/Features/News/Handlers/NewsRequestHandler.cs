using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking; // For IPacketHandler, IClientSession
using KnightOnline.Domain.Networking; // For Packet, ClientOpcodes, ServerOpcodes
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.LoginServer.Features.News.Handlers
{
    public class NewsRequestHandler : IPacketHandler
    {
        public ushort Opcode => ClientOpcodes.NewsRequest; // 0x00F6

        // private readonly ILogger<NewsRequestHandler> _logger; // Optional

        // public NewsRequestHandler(ILogger<NewsRequestHandler> logger = null)
        public NewsRequestHandler()
        {
            // _logger = logger;
        }

        public async Task HandlePacketAsync(IClientSession session, Packet requestPacket)
        {
            // _logger?.LogDebug($"Handling NewsRequest from session {session.SessionId}");
            System.Console.WriteLine($"Handling NewsRequest from session {session.SessionId}"); // Placeholder

            // 1. Define news content (hardcoded for now)
            // Knight Online news often includes a title and then content,
            // and can be quite long. Using a simple format here.
            // Newlines are typically handled by client rendering.
            string newsTitle = "Welcome to Our Server!";
            string newsBody = "Thank you for connecting to our Knight Online server. " +
                              "We are excited to have you here!\n\n" + //
 for newline in KO often
                              "Current Events:\n" +
                              "- Double EXP weekend!\n" +
                              "- 찾아주셔서 감사합니다 (Thank you for finding us)\n\n" +
                              "Please report any bugs on our forum at example.com.\n" +
                              "Have fun!";

            // Some KO clients expect a specific format like:
            // ushort titleLength; string title; ushort noticeLength; string notice;
            // For now, sending as one continuous block using Packet.WriteString
            // which itself is length-prefixed.
            // If a more complex structure (title + body as separate strings) is needed,
            // Packet class would need methods to write multiple strings or the handler would do it manually.
            // Let's assume a single string block for news for now.
            // The string itself can contain formatting cues like
 if the client supports them.
            string fullNewsContent = newsTitle + "\n" + newsBody;


            // 2. Construct the response packet
            var responsePacket = new Packet(ServerOpcodes.NewsResponse); // Uses the same opcode 0x00F6 for response
            responsePacket.WriteString(fullNewsContent); // Writes ushort length + UTF-8 string

            // _logger?.LogInformation($"Session {session.SessionId}: Sending news response.");
            System.Console.WriteLine($"Session {session.SessionId}: Sending news response."); // Placeholder
            await session.SendPacketAsync(responsePacket);
        }
    }
}
