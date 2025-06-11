using MediatR;
using KnightOnline.Application.DTOs.Server; // For ServerInfoDto
using System.Collections.Generic;

namespace KnightOnline.Application.Features.Server.Queries
{
    public class GetServerListQuery : IRequest<GetServerListQuery.Response>
    {
        // This 'Echo' property was in the original handler's response constructor.
        // It might be part of a client-side request for matching responses,
        // or a generic part of all queries. Including it for consistency.
        public ushort Echo { get; set; }

        public GetServerListQuery(ushort echo)
        {
            Echo = echo;
        }

        public class Response
        {
            public ushort Echo { get; }
            public List<ServerInfoDto> Servers { get; }

            public Response(ushort echo, List<ServerInfoDto> servers)
            {
                Echo = echo;
                Servers = servers ?? new List<ServerInfoDto>();
            }
        }
    }
}
