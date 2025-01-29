using Domain.Common.Exceptions;
using Domain.Entities.Common;

namespace Domain.Entities.Authorization
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        private readonly List<RolePermission> _permissions = new();
        public IReadOnlyCollection<RolePermission> Permissions => _permissions.AsReadOnly();

        protected Role() { } // For EF Core

        public Role(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new DomainException("Role name cannot be empty");

            Name = newName;
        }
    }
}
