using Domain.Common.Interfaces;

namespace Domain.Events.AccountEvents
{
    public class UserCreatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string Username { get; }
        public string Email { get; }
        public DateTime OccurredOn { get; }

        public UserCreatedEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
