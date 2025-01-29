using Application.Common.Models;
using Application.Common.Models.AuditLogs;

namespace Application.Common.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(
            string action,
            string entityName,
            string entityId,
            string changes,
            CancellationToken cancellationToken = default);

        Task<PaginatedList<AuditLogDto>> GetAuditLogsAsync(
            string entityName,
            string entityId,
            DateTime? fromDate,
            DateTime? toDate,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        Task<PaginatedList<AuditLogDto>> GetUserAuditLogsAsync(
            Guid userId,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
    }
}
