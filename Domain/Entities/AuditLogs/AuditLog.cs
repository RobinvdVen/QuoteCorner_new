using Domain.Entities.Authentication;
using Domain.Entities.Common;

namespace Domain.Entities.AuditLogs
{
    public class AuditLog : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string Action { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }
        public string Location { get; private set; }
        public bool Success { get; private set; }
        public string FailureReason { get; private set; }

        public ApplicationUser User { get; private set; }

        protected AuditLog() { } // For EF Core

        public AuditLog(
            Guid userId,
            string action,
            string ipAddress,
            string userAgent,
            string location,
            bool success,
            string failureReason = null)
        {
            UserId = userId;
            Action = action;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            Location = location;
            Success = success;
            FailureReason = failureReason;
        }
    }
}
