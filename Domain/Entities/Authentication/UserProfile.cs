using Domain.Entities.Common;

namespace Domain.Entities.Authentication
{
    public class UserProfile : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string TimeZone { get; private set; }
        public string Language { get; private set; }

        public ApplicationUser User { get; private set; }

        protected UserProfile() { } // For EF Core

        public UserProfile(
            Guid userId,
            string firstName,
            string lastName,
            string phoneNumber = null,
            string timeZone = "UTC",
            string language = "en")
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            TimeZone = timeZone;
            Language = language;
        }

        public void UpdateProfile(
            string firstName,
            string lastName,
            string phoneNumber,
            string timeZone,
            string language)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            TimeZone = timeZone;
            Language = language;
        }
    }
}
