using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Commands
{
    public record UpdateUserCommand : IRequest<Result<UserDto>>
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public string TimeZone { get; init; }
        public string Language { get; init; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public UpdateUserCommandHandler(
            IApplicationDbContext context,
            ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                return Result<UserDto>.Failure($"User with ID {request.Id} not found.");

            // Check if email is being changed and is unique
            if (user.Email != request.Email)
            {
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);

                if (emailExists)
                    return Result<UserDto>.Failure("Email already exists.");

                user.UpdateEmail(request.Email);
            }

            // Check if username is being changed and is unique
            if (user.Username != request.Username)
            {
                var usernameExists = await _context.Users
                    .AnyAsync(u => u.Username == request.Username && u.Id != request.Id, cancellationToken);

                if (usernameExists)
                    return Result<UserDto>.Failure("Username already exists.");

                user.UpdateUsername(request.Username);
            }

            user.Profile.UpdateProfile(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.TimeZone,
                request.Language
            );

            await _context.SaveChangesAsync(cancellationToken);

            return Result<UserDto>.Success(new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                PhoneNumber = user.Profile.PhoneNumber,
                TimeZone = user.Profile.TimeZone,
                Language = user.Profile.Language,
                IsActive = user.IsActive
            });
        }
    }
}
