using MediatR;
using System.Collections.Generic;
using KnightOnline.Application.DTOs.Server; // For ServerInfoDto

namespace KnightOnline.Application.Features.Server.Queries
{
    public class GetServerListQuery : IRequest<GetServerListQuery.Response>
    {
        public short Echo { get; set; } // The echo value from the client's request

        public GetServerListQuery(short echo)
        {
            Echo = echo;
        }

        // Nested class for the response
        public class Response
        {
            public short Echo { get; }
            public List<ServerInfoDto> Servers { get; }

            public Response(short echo, List<ServerInfoDto> servers)
            {
                Echo = echo;
                Servers = servers ?? new List<ServerInfoDto>();
            }
        }
    }
}
