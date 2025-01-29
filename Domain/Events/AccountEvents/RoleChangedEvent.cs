using Domain.Common.Interfaces;

namespace Domain.Events.AccountEvents
{
    public class RoleChangedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public string Action { get; } // "Added" or "Removed"
        public string ChangedBy { get; }
        public DateTime OccurredOn { get; }

        public RoleChangedEvent(Guid userId, Guid roleId, string action, string changedBy)
        {
            UserId = userId;
            RoleId = roleId;
            Action = action;
            ChangedBy = changedBy;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
