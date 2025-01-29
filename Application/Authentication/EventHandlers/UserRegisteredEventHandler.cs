using Application.Common.Interfaces;
using Domain.Events.AccountEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.EventHandlers
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<UserRegisteredEventHandler> _logger;

        public UserRegisteredEventHandler(
            IEmailService emailService,
            ILogger<UserRegisteredEventHandler> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _emailService.SendWelcomeEmailAsync(
                    notification.Email,
                    notification.Username,
                    cancellationToken);

                _logger.LogInformation("Welcome email sent to user {Username}", notification.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending welcome email to user {Username}", notification.Username);
            }
        }
    }
}
