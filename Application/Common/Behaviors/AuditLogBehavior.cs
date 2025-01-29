using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Common.Behaviors
{
    public class AuditLogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ICurrentUser _currentUser;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<AuditLogBehavior<TRequest, TResponse>> _logger;

        public AuditLogBehavior(
            ICurrentUser currentUser,
            IAuditLogService auditLogService,
            ILogger<AuditLogBehavior<TRequest, TResponse>> logger)
        {
            _currentUser = currentUser;
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Skip audit logging for queries
            if (typeof(TRequest).Name.EndsWith("Query"))
            {
                return await next();
            }

            var response = await next();

            try
            {
                var requestName = typeof(TRequest).Name;
                var entityName = requestName.Replace("Command", "");
                var action = GetActionFromCommandName(requestName);

                var changes = JsonSerializer.Serialize(new
                {
                    Request = request,
                    Response = response
                });

                await _auditLogService.LogAsync(
                    action,
                    entityName,
                    GetEntityId(request)?.ToString() ?? "N/A",
                    changes,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during audit logging for {RequestType}", typeof(TRequest).Name);
            }

            return response;
        }

        private string GetActionFromCommandName(string commandName)
        {
            if (commandName.StartsWith("Create")) return "Create";
            if (commandName.StartsWith("Update")) return "Update";
            if (commandName.StartsWith("Delete")) return "Delete";
            return "Other";
        }

        private object GetEntityId(TRequest request)
        {
            // Try to get Id property through reflection
            var idProperty = request.GetType().GetProperty("Id");
            return idProperty?.GetValue(request);
        }
    }
}
