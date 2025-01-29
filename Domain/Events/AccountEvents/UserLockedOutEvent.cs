using Domain.Common.Interfaces;

namespace Domain.Events.AccountEvents
{
    public class UserLockedOutEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public DateTime LockoutEnd { get; }
        public string IpAddress { get; }
        public int FailedAttempts { get; }
        public DateTime OccurredOn { get; }

        public UserLockedOutEvent(Guid userId, DateTime lockoutEnd, string ipAddress, int failedAttempts)
        {
            UserId = userId;
            LockoutEnd = lockoutEnd;
            IpAddress = ipAddress;
            FailedAttempts = failedAttempts;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
