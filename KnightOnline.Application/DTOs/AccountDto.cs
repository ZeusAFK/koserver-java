namespace KnightOnline.Application.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; } // Typically the domain ID
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
