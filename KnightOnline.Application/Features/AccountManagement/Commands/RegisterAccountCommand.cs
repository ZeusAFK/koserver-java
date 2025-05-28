using MediatR; // Assuming MediatR is added
using KnightOnline.Application.DTOs; // For AccountDto or a result type

namespace KnightOnline.Application.Features.AccountManagement.Commands
{
    public class RegisterAccountCommand : IRequest<AccountDto> // Or IRequest<Result<AccountDto>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
