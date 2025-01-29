using Application.Common.Interfaces;
using Domain.Common.Constants;
using Domain.Common.Interfaces;
using Domain.Entities.Authentication;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Application.Authentication.Commands
{
    public record RefreshTokenCommand : IRequest<LoginResponse>
    {
        public string? RefreshToken { get; init; }
        public string? IpAddress { get; init; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RefreshTokenCommandHandler(
            IApplicationDbContext context,
            ITokenGenerator tokenGenerator,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(r => r.User)
                .ThenInclude(u => u.Roles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(r => r.Token == request.RefreshToken, cancellationToken);

            if (refreshToken == null)
                throw new AuthenticationException("Invalid refresh token");

            if (refreshToken.ExpiresAt < _dateTimeProvider.UtcNow)
                throw new AuthenticationException("Refresh token expired");

            if (refreshToken.IsRevoked)
                throw new AuthenticationException("Refresh token revoked");

            var user = refreshToken.User;
            var roles = user.Roles.Select(r => r.Role.Name).ToList();

            var newAccessToken = _tokenGenerator.GenerateAccessToken(user, roles);
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            refreshToken.Revoke(request.IpAddress, newRefreshToken);

            user.RefreshTokens.Add(new RefreshToken(
                newRefreshToken,
                _dateTimeProvider.UtcNow.AddDays(AuthenticationConstants.RefreshTokenExpirationDays),
                user.Id,
                request.IpAddress
            ));

            await _context.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = AuthenticationConstants.AccessTokenExpirationMinutes * 60
            };
        }
    }

    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(v => v.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required");

            RuleFor(v => v.IpAddress)
                .NotEmpty().WithMessage("IP Address is required");
        }
    }

}
