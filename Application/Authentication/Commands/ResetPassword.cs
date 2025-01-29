using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Commands
{
    public record ResetPasswordCommand : IRequest<Result<Unit>>
    {
        public string Token { get; init; }
        public string NewPassword { get; init; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ResetPasswordCommandHandler(
            IApplicationDbContext context,
            IPasswordHasher passwordHasher,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token, cancellationToken);

            if (user == null || !user.IsPasswordResetTokenValid(request.Token))
            {
                return Result<Unit>.Failure("Invalid or expired reset token");
            }

            user.SetPasswordHash(_passwordHasher.HashPassword(request.NewPassword));
            user.ClearPasswordResetToken();
            user.ResetFailedLoginAttempts();

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}