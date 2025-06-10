using MediatR;

namespace KnightOnline.Application.Features.News.Queries
{
    public class GetNewsQuery : IRequest<GetNewsQuery.Response>
    {
        // No parameters needed for this query

        // Nested class for the response
        public class Response
        {
            public string Title { get; }
            public string Content { get; }

            public Response(string title, string content)
            {
                Title = title;
                Content = content;
            }
        }
    }
}
