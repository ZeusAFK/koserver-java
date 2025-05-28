using KnightOnline.Domain.SharedKernel;

namespace KnightOnline.Domain.Accounts
{
    public class UserId : ValueObject
    {
        public Guid Value { get; private set; }

        private UserId(Guid value) { Value = value; }

        public static UserId CreateUnique() => new UserId(Guid.NewGuid());
        public static UserId FromGuid(Guid value) => new UserId(value);

        // protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    }
}
