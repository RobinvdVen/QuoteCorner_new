using Domain.Entities.Authentication;
using Domain.Entities.Common;

namespace Domain.Entities.Authorization
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }

        public ApplicationUser User { get; private set; }
        public Role Role { get; private set; }

        protected UserRole() { } // For EF Core

        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
