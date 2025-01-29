using Domain.Entities.Common;

namespace Domain.Entities.Authorization
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; private set; }
        public Guid PermissionId { get; private set; }

        public Role Role { get; private set; }
        public Permission Permission { get; private set; }

        protected RolePermission() { } // For EF Core

        public RolePermission(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
