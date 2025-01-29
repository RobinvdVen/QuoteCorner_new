namespace Application.Common.Models.Authorization
{
    public class RoleDto : BaseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
