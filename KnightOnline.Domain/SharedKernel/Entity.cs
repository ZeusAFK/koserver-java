namespace KnightOnline.Domain.SharedKernel
{
    public abstract class Entity<TId> where TId : notnull
    {
        public TId Id { get; protected set; }

        protected Entity(TId id)
        {
            Id = id;
        }

        // Equals and GetHashCode overrides would typically go here
    }
}
