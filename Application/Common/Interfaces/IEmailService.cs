namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
        Task SendPasswordResetEmailAsync(string to, string username, string token, DateTime expiryDate, CancellationToken cancellationToken = default);
        Task SendWelcomeEmailAsync(string to, string username, CancellationToken cancellationToken = default);
    }
}
