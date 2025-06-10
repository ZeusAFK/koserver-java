using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KnightOnline.Application.Features.News.Queries
{
    public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, GetNewsQuery.Response>
    {
        private const string NewsTitle = "LoginNotice"; // From Java NewsHandler
        private const string NewsContent = "<empty>";   // From Java NewsHandler

        public GetNewsQueryHandler()
        {
            // No dependencies for now
        }

        public Task<GetNewsQuery.Response> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetNewsQuery.Response(NewsTitle, NewsContent);
            return Task.FromResult(response);
        }
    }
}
