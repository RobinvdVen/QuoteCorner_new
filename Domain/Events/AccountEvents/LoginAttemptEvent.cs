using Domain.Common.Interfaces;

namespace Domain.Events.AccountEvents
{
    public class LoginAttemptEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public bool Success { get; }
        public string IpAddress { get; }
        public string UserAgent { get; }
        public string Location { get; }
        public string FailureReason { get; }
        public DateTime OccurredOn { get; }

        public LoginAttemptEvent(
            Guid userId,
            bool success,
            string ipAddress,
            string userAgent,
            string location,
            string failureReason = null)
        {
            UserId = userId;
            Success = success;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            Location = location;
            FailureReason = failureReason;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
