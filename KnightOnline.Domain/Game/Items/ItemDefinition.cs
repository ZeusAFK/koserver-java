using KnightOnline.Domain.SharedKernel;
namespace KnightOnline.Domain.Game.Items
{
    public class ItemDefinition : Entity<int> // Or some other ID type
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        // Stats, requirements, etc.
        public ItemDefinition(int id, string name, string description) : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
