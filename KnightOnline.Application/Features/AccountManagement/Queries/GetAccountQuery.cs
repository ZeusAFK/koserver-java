using MediatR;
using KnightOnline.Application.DTOs;

namespace KnightOnline.Application.Features.AccountManagement.Queries
{
    public class GetAccountQuery : IRequest<AccountDto>
    {
        public Guid UserId { get; set; }
    }
}
