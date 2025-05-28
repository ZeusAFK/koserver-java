using KnightOnline.Domain.SharedKernel;

namespace KnightOnline.Domain.Players
{
    public class CharacterId : ValueObject
    {
        public int Value { get; private set; }

        private CharacterId(int value) { Value = value; }
        public static CharacterId FromInt(int value) => new CharacterId(value);
         // protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    }
}
