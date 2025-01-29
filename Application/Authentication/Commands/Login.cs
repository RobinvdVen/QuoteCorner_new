using Application.Common.Interfaces;
using Domain.Common.Constants;
using Domain.Common.Interfaces;
using Domain.Entities.Authentication;
using Domain.Events.AccountEvents;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Application.Authentication.Commands
{
    public record LoginCommand : IRequest<LoginResponse>
    {
        public string? Email { get; init; }
        public string? Password { get; init; }
        public string? IpAddress { get; init; }
        public string? UserAgent { get; init; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IGeoLocationService _geoLocationService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public LoginCommandHandler(
            IApplicationDbContext context,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IGeoLocationService geoLocationService,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _geoLocationService = geoLocationService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
                throw new AuthenticationException("Invalid credentials");

            if (user.IsLockedOut())
                throw new AuthenticationException("Account is locked out");

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                user.IncrementFailedLoginAttempts();

                if (user.FailedLoginAttempts >= AuthenticationConstants.MaxFailedLoginAttempts)
                {
                    user.LockAccount(TimeSpan.FromMinutes(AuthenticationConstants.LockoutDurationInMinutes));
                }

                await _context.SaveChangesAsync(cancellationToken);
                throw new AuthenticationException("Invalid credentials");
            }

            var location = await _geoLocationService.GetLocationFromIpAddress(request.IpAddress);
            var roles = user.Roles.Select(r => r.Role.Name).ToList();

            var accessToken = _tokenGenerator.GenerateAccessToken(user, roles);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken(
                refreshToken,
                _dateTimeProvider.UtcNow.AddDays(AuthenticationConstants.RefreshTokenExpirationDays),
                user.Id,
                request.IpAddress
            ));

            user.RecordSuccessfulLogin();

            var loginEvent = new LoginAttemptEvent(
                user.Id,
                true,
                request.IpAddress,
                request.UserAgent,
                location);

            // TODO: Publish domain event

            await _context.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = AuthenticationConstants.AccessTokenExpirationMinutes * 60
            };
        }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not in correct format");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required");

            RuleFor(v => v.IpAddress)
                .NotEmpty().WithMessage("IP Address is required");

            RuleFor(v => v.UserAgent)
                .NotEmpty().WithMessage("User Agent is required");
        }
    }

    public record LoginResponse
    {
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }
        public int ExpiresIn { get; init; }
    }
}
