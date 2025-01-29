namespace Domain.Common.Interfaces
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }

}
