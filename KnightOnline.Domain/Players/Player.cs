using KnightOnline.Domain.SharedKernel;
using KnightOnline.Domain.Accounts; // Assuming a player is linked to an Account's UserId

namespace KnightOnline.Domain.Players
{
    public class Player : Entity<CharacterId> // Example of Aggregate Root
    {
        public UserId AccountId { get; private set; }
        public string Name { get; private set; }
        // Other properties like Level, Class, Inventory etc.

        public Player(CharacterId id, UserId accountId, string name) : base(id)
        {
            AccountId = accountId;
            Name = name;
        }
    }
}
