namespace Domain.Events.AccountEvents
{
    public class PasswordResetRequestedEvent : DomainEvent
    {
        public Guid UserId { get; }

        public PasswordResetRequestedEvent(Guid userId) : base()
        {
            UserId = userId;
        }
    }
}
