using MediatR;
using KnightOnline.Application.DTOs.Authentication; // For LoginResultDto

namespace KnightOnline.Application.Features.Authentication.Commands
{
    public class LoginCommand : IRequest<LoginResultDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
