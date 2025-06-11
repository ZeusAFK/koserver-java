using MediatR;
using KnightOnline.Application.Contracts.Infrastructure; // For IServerDetailRepository
using KnightOnline.Application.DTOs.Server; // For ServerInfoDto
// using KnightOnline.Domain.Client; // Not strictly needed if ServerDetail isn't directly used here after mapping
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System; // For ArgumentNullException

namespace KnightOnline.Application.Features.Server.Queries
{
    public class GetServerListQueryHandler : IRequestHandler<GetServerListQuery, GetServerListQuery.Response>
    {
        private readonly IServerDetailRepository _serverDetailRepository;

        public GetServerListQueryHandler(IServerDetailRepository serverDetailRepository)
        {
            _serverDetailRepository = serverDetailRepository ?? throw new ArgumentNullException(nameof(serverDetailRepository));
        }

        public async Task<GetServerListQuery.Response> Handle(GetServerListQuery request, CancellationToken cancellationToken)
        {
            var serverDetailsDomain = await _serverDetailRepository.GetAllServersAsync();

            var serverInfoDtos = new List<ServerInfoDto>();
            if (serverDetailsDomain != null)
            {
                foreach (var domainServer in serverDetailsDomain)
                {
                    var dto = new ServerInfoDto
                    {
                        ServerId = domainServer.Id,
                        Name = domainServer.Name,
                        Ip = domainServer.IpAddress,
                        Port = domainServer.Port, // Added Port mapping
                        UserCount = domainServer.CurrentUsers,
                        UserMax = domainServer.MaxUsers,
                        Category = domainServer.ServerCategory switch
                        {
                            "Premium" => 2,
                            "Normal" => 1,
                            _ => 0
                        },
                        UserMaxFree = domainServer.UserMaxFree // Corrected mapping
                    };
                    serverInfoDtos.Add(dto);
                }
            }

            var response = new GetServerListQuery.Response(request.Echo, serverInfoDtos);
            return response;
        }
    }
}
