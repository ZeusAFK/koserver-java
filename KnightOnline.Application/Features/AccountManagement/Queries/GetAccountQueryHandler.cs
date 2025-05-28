using MediatR;
using KnightOnline.Application.Contracts.Infrastructure;
using KnightOnline.Application.DTOs;
using KnightOnline.Domain.Accounts;

namespace KnightOnline.Application.Features.AccountManagement.Queries
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDto>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDto> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(UserId.FromGuid(request.UserId));
            // return _mapper.Map<AccountDto>(account);
            if (account == null) return null; // Or throw exception
            return new AccountDto { Id = account.Id.Value, Username = account.Username, Email = account.Email }; // Placeholder
        }
    }
}
