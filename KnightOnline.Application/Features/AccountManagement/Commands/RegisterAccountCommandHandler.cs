using MediatR;
using KnightOnline.Application.Contracts.Infrastructure;
using KnightOnline.Domain.Accounts;
using KnightOnline.Application.DTOs; // For AccountDto or a custom result type

namespace KnightOnline.Application.Features.AccountManagement.Commands
{
    public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, AccountDto>
    {
        private readonly IAccountRepository _accountRepository;
        // private readonly IMapper _mapper; // If using AutoMapper

        public RegisterAccountCommandHandler(IAccountRepository accountRepository /*, IMapper mapper*/)
        {
            _accountRepository = accountRepository;
            // _mapper = mapper;
        }

        public async Task<AccountDto> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            // Placeholder logic:
            // var account = Account.Create(request.Username, request.Password, request.Email);
            // await _accountRepository.AddAsync(account);
            // return _mapper.Map<AccountDto>(account);
            return await Task.FromResult(new AccountDto { Username = request.Username, Email = request.Email }); // Placeholder
        }
    }
}
