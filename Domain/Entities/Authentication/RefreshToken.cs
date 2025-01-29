using Domain.Entities.Common;

namespace Domain.Entities.Authentication
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public Guid UserId { get; private set; }
        public string CreatedByIp { get; private set; }
        public bool IsRevoked { get; private set; }
        public string RevokedByIp { get; private set; }
        public string ReplacedByToken { get; private set; }

        public ApplicationUser User { get; private set; }

        protected RefreshToken() { } // For EF Core

        public RefreshToken(string token, DateTime expiresAt, Guid userId, string createdByIp)
        {
            Token = token;
            ExpiresAt = expiresAt;
            UserId = userId;
            CreatedByIp = createdByIp;
        }

        public void Revoke(string ipAddress, string replacedByToken = null)
        {
            IsRevoked = true;
            RevokedByIp = ipAddress;
            ReplacedByToken = replacedByToken;
        }
    }

}
