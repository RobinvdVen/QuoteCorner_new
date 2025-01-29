using Domain.Common.Interfaces;

namespace Domain.Events.AccountEvents
{
    public class ProfileUpdatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public DateTime OccurredOn { get; }

        public ProfileUpdatedEvent(Guid userId)
        {
            UserId = userId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
