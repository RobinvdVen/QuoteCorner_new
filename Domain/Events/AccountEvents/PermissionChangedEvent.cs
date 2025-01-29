using Domain.Common.Interfaces;

namespace Domain.Events.AccountEvents
{
    public class PermissionChangedEvent : IDomainEvent
    {
        public Guid RoleId { get; }
        public Guid PermissionId { get; }
        public string Action { get; } // "Granted" or "Revoked"
        public DateTime OccurredOn { get; }

        public PermissionChangedEvent(Guid roleId, Guid permissionId, string action)
        {
            RoleId = roleId;
            PermissionId = permissionId;
            Action = action;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
