using KnightOnline.Domain.SharedKernel;
using KnightOnline.Domain.Enums; // For AccountAuthority
using System; // For DateTime

namespace KnightOnline.Domain.Accounts
{
    public class Account : Entity<UserId> // Example of Aggregate Root
    {
        public string Username { get; private set; }
        public string Email { get; private set; }

        // SECURITY TODO: This should store a HASHED password, not plain text.
        // The salt should also be stored. For this migration step, we mirror
        // the Java project's apparent plain text storage, but this is a P0 security issue.
        private string _password; // Plain text for now, reflecting Java project's apparent state

        public AccountAuthority Authority { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? LastAccessedDate { get; private set; }
        // public int Nation { get; private set; } // Example from Java AccountEntity

        // Constructor for creating a new account
        public Account(UserId id, string username, string email) : base(id)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            Username = username;
            Email = email; // Consider email validation
            Authority = AccountAuthority.NORMAL; // Default authority
            CreationDate = DateTime.UtcNow;
            _password = string.Empty; // Password should be set explicitly
        }

        // Constructor for EF Core hydration (if needed, though EF can map to private setters)
        // protected Account(UserId id) : base(id) {}

        public void SetPassword(string plainTextPassword)
        {
            // SECURITY TODO: Hash the password here before storing.
            // For now, direct assignment to match Java's apparent behavior.
            if (string.IsNullOrEmpty(plainTextPassword))
                throw new ArgumentException("Password cannot be empty.", nameof(plainTextPassword));
            _password = plainTextPassword;
        }

        public bool VerifyPassword(string plainTextPassword)
        {
            // SECURITY TODO: Compare provided password against stored HASH.
            // For now, direct comparison.
            return _password == plainTextPassword;
        }

        public void SetAuthority(AccountAuthority newAuthority)
        {
            Authority = newAuthority;
            // Potentially raise a domain event if authority changes are significant
        }

        public void SetLastAccessedDate(DateTime accessDate)
        {
            LastAccessedDate = accessDate;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            Email = email;
        }

        // Other domain logic methods for Account aggregate
        // For example:
        // public void Ban() { Authority = AccountAuthority.BANNED; }
        // public void Unban() { Authority = AccountAuthority.NORMAL; }
    }
}
