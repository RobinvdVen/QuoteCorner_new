namespace Application.Common.Models.Authorization
{
    public class PermissionDto : BaseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}
