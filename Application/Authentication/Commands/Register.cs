using Application.Common.Constants;
using Application.Common.Interfaces;
using Domain.Common.Constants;
using Domain.Entities.Authentication;
using Domain.Entities.Authorization;
using Domain.Rules;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Commands
{
    public record RegisterCommand : IRequest<RegisterResponse>
    {
        public string? Email { get; init; }
        public string? Username { get; init; }
        public string? Password { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUser _currentUser;

        public RegisterCommandHandler(
            IApplicationDbContext context,
            IPasswordHasher passwordHasher,
            ICurrentUser currentUser)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _currentUser = currentUser;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicate email
            if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
                throw new ValidationException(new[] { new ValidationFailure(nameof(request.Email), ErrorMessages.DuplicateEmail) });

            // Check for duplicate username
            if (await _context.Users.AnyAsync(u => u.Username == request.Username, cancellationToken))
                throw new ValidationException(new[] { new ValidationFailure(nameof(request.Username), ErrorMessages.DuplicateUsername) });

            var user = new ApplicationUser(request.Username, request.Email);
            user.SetPasswordHash(_passwordHasher.HashPassword(request.Password));

            var userProfile = new UserProfile(
                user.Id,
                request.FirstName,
                request.LastName,
                timeZone: AuthenticationConstants.DefaultTimeZone,
                language: AuthenticationConstants.DefaultLanguage
            );

            _context.Users.Add(user);
            _context.UserProfiles.Add(userProfile);

            // Add default user role
            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "User", cancellationToken);

            if (defaultRole != null)
            {
                _context.UserRoles.Add(new UserRole(user.Id, defaultRole.Id));
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
    }

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty().WithMessage(ErrorMessages.EmailRequired)
                .EmailAddress().WithMessage(ErrorMessages.InvalidEmailFormat);

            RuleFor(v => v.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(UsernameRules.MinimumLength).WithMessage($"Username must be at least {UsernameRules.MinimumLength} characters")
                .MaximumLength(UsernameRules.MaximumLength).WithMessage($"Username must not exceed {UsernameRules.MaximumLength} characters")
                .Matches($"^[{UsernameRules.AllowedCharacters}]+$").WithMessage("Username contains invalid characters");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage(ErrorMessages.PasswordRequired)
                .MinimumLength(PasswordRules.MinimumLength).WithMessage($"Password must be at least {PasswordRules.MinimumLength} characters")
                .MaximumLength(PasswordRules.MaximumLength).WithMessage($"Password must not exceed {PasswordRules.MaximumLength} characters")
                .Must(password => password.Any(char.IsUpper)).When(x => PasswordRules.RequireUppercase).WithMessage("Password must contain at least one uppercase letter")
                .Must(password => password.Any(char.IsLower)).When(x => PasswordRules.RequireLowercase).WithMessage("Password must contain at least one lowercase letter")
                .Must(password => password.Any(char.IsDigit)).When(x => PasswordRules.RequireDigit).WithMessage("Password must contain at least one number")
                .Must(password => password.Any(ch => !char.IsLetterOrDigit(ch))).When(x => PasswordRules.RequireNonAlphanumeric).WithMessage("Password must contain at least one special character");

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");
        }
    }

    public record RegisterResponse
    {
        public Guid Id { get; init; }
        public string? Username { get; init; }
        public string? Email { get; init; }
    }
}
