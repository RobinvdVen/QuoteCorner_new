using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Authentication;
using Application.Common.Models.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries
{
    public record GetUserByIdQuery : IRequest<Result<UserDetailDto>>
    {
        public Guid Id { get; init; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDetailDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetUserByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<UserDetailDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.Roles)
                    .ThenInclude(r => r.Role)
                        .ThenInclude(r => r.Permissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                return Result<UserDetailDto>.Failure($"User with ID {request.Id} not found.");

            var userDto = new UserDetailDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                PhoneNumber = user.Profile.PhoneNumber,
                TimeZone = user.Profile.TimeZone,
                Language = user.Profile.Language,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                FailedLoginAttempts = user.FailedLoginAttempts,
                LockoutEnd = user.LockoutEnd,
                Roles = user.Roles.Select(r => new RoleDto
                {
                    Id = r.Role.Id,
                    Name = r.Role.Name,
                    Description = r.Role.Description,
                    Permissions = r.Role.Permissions
                        .Select(p => new PermissionDto
                        {
                            Id = p.Permission.Id,
                            Name = p.Permission.Name,
                            Description = p.Permission.Description,
                            Category = p.Permission.Category
                        })
                        .ToList()
                }).ToList()
            };

            return Result<UserDetailDto>.Success(userDto);
        }
    }

}
