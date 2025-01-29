using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries
{
    public record GetUsersQuery : IRequest<Result<PaginatedList<UserDto>>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? SearchTerm { get; init; }
        public string? SortBy { get; init; }
        public bool SortDescending { get; init; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PaginatedList<UserDto>>>
    {
        private readonly IApplicationDbContext _context;

        public GetUsersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PaginatedList<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Users
                .Include(u => u.Profile)
                .Include(u => u.Roles)
                    .ThenInclude(r => r.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(u =>
                    u.Username.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.Profile.FirstName.ToLower().Contains(searchTerm) ||
                    u.Profile.LastName.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            query = request.SortBy?.ToLower() switch
            {
                "username" => request.SortDescending
                    ? query.OrderByDescending(u => u.Username)
                    : query.OrderBy(u => u.Username),
                "email" => request.SortDescending
                    ? query.OrderByDescending(u => u.Email)
                    : query.OrderBy(u => u.Email),
                "firstname" => request.SortDescending
                    ? query.OrderByDescending(u => u.Profile.FirstName)
                    : query.OrderBy(u => u.Profile.FirstName),
                "lastname" => request.SortDescending
                    ? query.OrderByDescending(u => u.Profile.LastName)
                    : query.OrderBy(u => u.Profile.LastName),
                _ => query.OrderBy(u => u.Username)
            };

            var users = await query
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    PhoneNumber = u.Profile.PhoneNumber,
                    TimeZone = u.Profile.TimeZone,
                    Language = u.Profile.Language,
                    IsActive = u.IsActive,
                    LastLoginDate = u.LastLoginDate,
                    Roles = u.Roles.Select(r => r.Role.Name).ToList()
                })
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return Result<PaginatedList<UserDto>>.Success(users);
        }
    }
}