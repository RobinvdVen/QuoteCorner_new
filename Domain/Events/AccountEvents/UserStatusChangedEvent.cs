namespace Domain.Events.AccountEvents
{
    public class UserStatusChangedEvent : DomainEvent
    {
        public Guid UserId { get; }
        public bool IsActive { get; }
        public string Reason { get; }

        public UserStatusChangedEvent(Guid userId, bool isActive, string reason) : base()
        {
            UserId = userId;
            IsActive = isActive;
            Reason = reason;
        }
    }
}
