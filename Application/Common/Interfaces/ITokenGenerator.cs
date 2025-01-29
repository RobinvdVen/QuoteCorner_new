using Domain.Entities.Authentication;

namespace Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(ApplicationUser user, IList<string> roles);
        string GenerateRefreshToken();
        string GeneratePasswordResetToken();
    }
}
