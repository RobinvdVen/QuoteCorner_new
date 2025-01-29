using Domain.Entities.AuditLogs;
using Domain.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.AccountEvents
{
    public class UserRegisteredEvent : DomainEvent
    {
        public Guid UserId { get; }
        public string Username { get; }
        public string Email { get; }

        public UserRegisteredEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }
}