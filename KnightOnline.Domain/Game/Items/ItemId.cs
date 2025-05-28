using KnightOnline.Domain.SharedKernel;
namespace KnightOnline.Domain.Game.Items
{
    public class ItemId : ValueObject
    {
        public int Value { get; private set; }
        private ItemId(int value) { Value = value; }
        public static ItemId FromInt(int value) => new ItemId(value);
        // protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    }
}
