using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Infrastructure; // For IAccountRepository
using KnightOnline.Application.DTOs.Authentication;    // For LoginResultDto
using KnightOnline.Domain.Accounts;                   // For Account, UserId
using KnightOnline.Domain.Enums;                      // For LoginResult, AccountAuthority
using KnightOnline.Infrastructure.Configuration;      // For AppSettings (to get AutoCreateAccount)
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.Application.Features.Authentication.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AppSettings _appSettings;
        // private readonly ILogger<LoginCommandHandler> _logger; // Optional

        private const short FixedLoginResponseValue = 7857; // From Java LoginRequestHandler

        // public LoginCommandHandler(IAccountRepository accountRepository, AppSettings appSettings, ILogger<LoginCommandHandler> logger)
        public LoginCommandHandler(IAccountRepository accountRepository, AppSettings appSettings)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            // _logger = logger;
        }

        public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // _logger?.LogInformation($"Attempting login for user: {request.Username}");
            Console.WriteLine($"Attempting login for user: {request.Username}"); // Placeholder

            Account? account = await _accountRepository.GetByUsernameAsync(request.Username);

            if (account == null && _appSettings.LoginServer.AutoCreateAccount)
            {
                // _logger?.LogInformation($"Account {request.Username} not found. Auto-creating account.");
                Console.WriteLine($"Account {request.Username} not found. Auto-creating account.");// Placeholder

                // Create new account
                // UserId.CreateUnique() generates a new GUID for the ID.
                // Email is set to a placeholder, consider if email is required or can be null initially.
                account = new Account(UserId.CreateUnique(), request.Username, request.Username + "@placeholder.com");
                account.SetPassword(request.Password); // SECURITY TODO: Hash this password.
                // Authority is already set to NORMAL by default in Account constructor.
                // CreationDate is set by constructor.

                await _accountRepository.AddAsync(account);
                // Ensure changes are saved if AddAsync doesn't do it. This depends on UoW pattern.
                // For EF Core, if _dbContext.SaveChangesAsync() is not in AddAsync, it would be needed here or at end of use case.
                // Example: await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            if (account == null)
            {
                // _logger?.LogWarning($"Login failed: Account {request.Username} not found.");
                Console.WriteLine($"Login failed: Account {request.Username} not found.");// Placeholder
                return new LoginResultDto(LoginResult.AuthNotFound, FixedLoginResponseValue, request.Username);
            }

            // SECURITY TODO: Implement proper password hashing and comparison.
            if (!account.VerifyPassword(request.Password))
            {
                // _logger?.LogWarning($"Login failed: Invalid password for account {request.Username}.");
                Console.WriteLine($"Login failed: Invalid password for account {request.Username}.");// Placeholder
                return new LoginResultDto(LoginResult.AuthInvalid, FixedLoginResponseValue, request.Username);
            }

            if (account.Authority == AccountAuthority.BANNED)
            {
                // _logger?.LogWarning($"Login failed: Account {request.Username} is banned.");
                Console.WriteLine($"Login failed: Account {request.Username} is banned.");// Placeholder
                return new LoginResultDto(LoginResult.AuthBanned, FixedLoginResponseValue, request.Username);
            }

            // TODO: Check for already in-game status (LoginResult.AuthInGame).
            // This might involve another service or repository (e.g., IActiveSessionRepository).

            // Successful login
            account.SetLastAccessedDate(DateTime.UtcNow);
            await _accountRepository.UpdateAsync(account);
            // Ensure changes are saved if UpdateAsync doesn't do it.
            // Example: await _unitOfWork.SaveChangesAsync(cancellationToken);

            // _logger?.LogInformation($"Login successful for account {request.Username}.");
            Console.WriteLine($"Login successful for account {request.Username}.");// Placeholder
            return new LoginResultDto(LoginResult.AuthSuccess, FixedLoginResponseValue, request.Username);
        }
    }
}
