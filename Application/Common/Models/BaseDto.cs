using Application.Common.Models.AuditLogs;

namespace Application.Common.Models
{
    public abstract class BaseDto : AuditableDto
    {
        public Guid Id { get; set; }
    }
}
