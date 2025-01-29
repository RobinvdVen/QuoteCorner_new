using Domain.Entities.Common;

namespace Domain.Entities.Authorization
{
    public class Permission : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }

        protected Permission() { } // For EF Core

        public Permission(string name, string description, string category)
        {
            Name = name;
            Description = description;
            Category = category;
        }
    }
}
