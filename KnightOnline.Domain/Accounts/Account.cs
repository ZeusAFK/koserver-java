using KnightOnline.Domain.SharedKernel;

namespace KnightOnline.Domain.Accounts
{
    public class Account : Entity<UserId> // Example of Aggregate Root
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        // Other properties

        public Account(UserId id, string username, string email) : base(id)
        {
            Username = username;
            Email = email;
        }

        // Factory method, domain logic methods
    }
}
