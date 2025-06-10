using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Application.DTOs.Server; // For ServerInfoDto

namespace KnightOnline.Application.Features.Server.Queries
{
    public class GetServerListQueryHandler : IRequestHandler<GetServerListQuery, GetServerListQuery.Response>
    {
        // In a real application, this might inject a service or repository to fetch server data,
        // or IOptions<ServerListConfig> to get it from configuration.
        public GetServerListQueryHandler()
        {
            // No dependencies for now, using hardcoded data.
        }

        public Task<GetServerListQuery.Response> Handle(GetServerListQuery request, CancellationToken cancellationToken)
        {
            // Placeholder: Hardcoded server list
            var servers = new List<ServerInfoDto>
            {
                new ServerInfoDto
                {
                    ServerId = 1,
                    Name = "Ares",
                    Ip = "127.0.0.1", // Or your server's public IP
                    UserCount = 150,  // Example user count
                    UserMax = 1000,
                    UserMaxFree = 800,
                    Category = 1      // Example category
                },
                new ServerInfoDto
                {
                    ServerId = 2,
                    Name = "Diez",
                    Ip = "127.0.0.1",
                    UserCount = 250,
                    UserMax = 1000,
                    UserMaxFree = 800,
                    Category = 1
                },
                new ServerInfoDto
                {
                    ServerId = 3,
                    Name = "Gordion",
                    Ip = "127.0.0.1",
                    UserCount = 100,
                    UserMax = 1200,
                    UserMaxFree = 900,
                    Category = 2
                }
            };

            var response = new GetServerListQuery.Response(request.Echo, servers);

            return Task.FromResult(response);
        }
    }
}
