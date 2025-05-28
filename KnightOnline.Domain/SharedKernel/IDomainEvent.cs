namespace KnightOnline.Domain.SharedKernel
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
