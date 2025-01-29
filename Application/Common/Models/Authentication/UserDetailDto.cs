using Application.Common.Models.Authorization;

namespace Application.Common.Models.Authentication
{
    public class UserDetailDto : UserDto
    {
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public List<RoleDto>? Roles { get; set; } = new();
    }
}
