using Domain.Common.Exceptions;
using Domain.Entities.AuditLogs;
using Domain.Entities.Authorization;
using Domain.Entities.Common;
using Domain.Events.AccountEvents;

namespace Domain.Entities.Authentication
{
    public class ApplicationUser : BaseEntity
    {
        public const int MaxFailedAttempts = 5;
        public const int LockoutDurationMinutes = 30;

        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LockoutEnd { get; private set; }
        public string PasswordResetToken { get; private set; }
        public DateTime? PasswordResetTokenExpiresAt { get; private set; }
        public UserProfile Profile { get; private set; }

        private readonly List<UserRole> _roles = new();
        public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
        public ICollection<AuditLog> AuditLogs { get; private set; }

        protected ApplicationUser() { } // For EF Core

        public ApplicationUser(string username, string email)
        {
            Username = username;
            Email = email;
            IsActive = true;
            RefreshTokens = new List<RefreshToken>();
            AuditLogs = new List<AuditLog>();
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
                throw new DomainException("Email cannot be empty");

            Email = newEmail;
        }

        public void UpdateUsername(string newUsername)
        {
            if (string.IsNullOrEmpty(newUsername))
                throw new DomainException("Username cannot be empty");

            Username = newUsername;
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrEmpty(passwordHash))
                throw new DomainException("Password hash cannot be empty");

            PasswordHash = passwordHash;
        }

        public void IncrementFailedLoginAttempts()
        {
            FailedLoginAttempts++;
        }

        public void ResetFailedLoginAttempts()
        {
            FailedLoginAttempts = 0;
            LockoutEnd = null;
        }

        public void LockAccount(TimeSpan duration)
        {
            LockoutEnd = DateTime.UtcNow.Add(duration);
        }

        public bool IsLockedOut() => LockoutEnd?.CompareTo(DateTime.UtcNow) > 0;

        public void RecordSuccessfulLogin()
        {
            LastLoginDate = DateTime.UtcNow;
            ResetFailedLoginAttempts();
        }

        public void RecordLoginAttempt(bool success)
        {
            if (!success)
            {
                IncrementFailedLoginAttempts();

                if (FailedLoginAttempts >= MaxFailedAttempts)
                {
                    LockAccount(TimeSpan.FromMinutes(LockoutDurationMinutes));
                }
            }
            else
            {
                RecordSuccessfulLogin();
            }
        }

        public bool AddToRole(Role role, string changedBy)
        {
            if (_roles.Any(r => r.RoleId == role.Id))
                return false;

            var userRole = new UserRole(Id, role.Id);
            _roles.Add(userRole);

            AddDomainEvent(new RoleChangedEvent(Id, role.Id, "Added", changedBy));
            return true;
        }

        public bool RemoveFromRole(Role role, string changedBy)
        {
            var userRole = _roles.FirstOrDefault(r => r.RoleId == role.Id);
            if (userRole == null)
                return false;

            _roles.Remove(userRole);
            AddDomainEvent(new RoleChangedEvent(Id, role.Id, "Removed", changedBy));
            return true;
        }

        public bool HasPermission(string permissionName)
        {
            return Roles.Any(ur => ur.Role.Permissions
                .Any(rp => rp.Permission.Name == permissionName));
        }

        public void SetPasswordResetToken(string token, DateTime expiryDate)
        {
            if (string.IsNullOrEmpty(token))
                throw new DomainException("Password reset token cannot be empty");

            PasswordResetToken = token;
            PasswordResetTokenExpiresAt = expiryDate;
            AddDomainEvent(new PasswordResetRequestedEvent(Id));
        }

        public void ClearPasswordResetToken()
        {
            PasswordResetToken = null;
            PasswordResetTokenExpiresAt = null;
        }

        public bool IsPasswordResetTokenValid(string token)
        {
            return PasswordResetToken == token &&
                   PasswordResetTokenExpiresAt > DateTime.UtcNow;
        }

        public void SetActiveStatus(bool isActive, string reason = null)
        {
            if (IsActive == isActive) return;

            IsActive = isActive;
            AddDomainEvent(new UserStatusChangedEvent(Id, isActive, reason));
        }
    }
}
