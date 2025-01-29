using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly ICurrentUser _currentUser;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger,
            ICurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUser.Id ?? Guid.Empty;
            var userName = _currentUser.Username ?? "Anonymous";

            _logger.LogInformation(
                "Handling {RequestName} for user {UserName} ({UserId})",
                requestName, userName, userId);

            var response = await next();

            _logger.LogInformation(
                "Handled {RequestName} for user {UserName} ({UserId})",
                requestName, userName, userId);

            return response;
        }
    }
}
