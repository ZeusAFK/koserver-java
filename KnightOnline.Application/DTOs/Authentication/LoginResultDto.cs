using KnightOnline.Domain.Enums; // For LoginResult enum

namespace KnightOnline.Application.DTOs.Authentication
{
    public class LoginResultDto
    {
        public LoginResult ResultCode { get; set; }
        public short FixedValue { get; set; } // The hardcoded 7857
        public string Username { get; set; }

        // Optional: A flag for overall success for easier checking
        public bool IsSuccess => ResultCode == LoginResult.AuthSuccess;

        public LoginResultDto(LoginResult resultCode, short fixedValue, string username)
        {
            ResultCode = resultCode;
            FixedValue = fixedValue;
            Username = username;
        }
    }
}
