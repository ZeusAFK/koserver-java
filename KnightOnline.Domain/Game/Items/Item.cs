using KnightOnline.Domain.SharedKernel;
namespace KnightOnline.Domain.Game.Items
{
    public class Item : Entity<ItemId>
    {
        public string Name { get; private set; }
        public Item(ItemId id, string name) : base(id) { Name = name; }
    }
}
