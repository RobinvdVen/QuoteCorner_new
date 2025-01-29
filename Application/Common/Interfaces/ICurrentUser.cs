namespace Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        Guid? Id { get; }
        string Username { get; }
        string Email { get; }
        IEnumerable<string> Roles { get; }
        bool IsAuthenticated { get; }
        string IpAddress { get; }
        string UserAgent { get; }
    }
}
