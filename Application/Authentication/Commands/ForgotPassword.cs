using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Commands
{
    public record ForgotPasswordCommand : IRequest<Result<Unit>>
    {
        public string Email { get; init; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ForgotPasswordCommandHandler(
            IApplicationDbContext context,
            IEmailService emailService,
            ITokenGenerator tokenGenerator,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _emailService = emailService;
            _tokenGenerator = tokenGenerator;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<Unit>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
                return Result<Unit>.Success(Unit.Value); // Don't reveal user existence

            var token = _tokenGenerator.GeneratePasswordResetToken();
            var expiryDate = DateTime.UtcNow.AddHours(24);

            user.SetPasswordResetToken(token, expiryDate);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendPasswordResetEmailAsync(
                user.Email,
                user.Username,
                token,
                expiryDate,
                cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
