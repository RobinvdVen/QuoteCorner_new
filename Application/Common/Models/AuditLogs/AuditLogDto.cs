namespace Application.Common.Models.AuditLogs
{
    public class AuditLogDto : BaseDto
    {
        public string? Action { get; set; }
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? Changes { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
