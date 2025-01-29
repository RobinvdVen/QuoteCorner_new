namespace Domain.Common.Constants
{
    public static class AuthenticationConstants
    {
        // Account Security
        public const int MaxFailedLoginAttempts = 5;
        public const int LockoutDurationInMinutes = 30;

        // Password Rules
        public const int MinimumPasswordLength = 8;
        public const int MaximumPasswordLength = 128;
        public const string PasswordRequirementsMessage =
            "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character";

        // Username Rules
        public const int MinimumUsernameLength = 3;
        public const int MaximumUsernameLength = 50;
        public const string UsernameAllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

        // Email Rules
        public const int EmailMaxLength = 256;

        // Token Configuration
        public const int AccessTokenExpirationMinutes = 30;
        public const int RefreshTokenExpirationDays = 7;
        public const int PasswordResetTokenExpirationHours = 24;
        public const string TokenType = "Bearer";

        // Claims
        public const string ClaimTypes_UserId = "uid";
        public const string ClaimTypes_Username = "username";
        public const string ClaimTypes_Email = "email";
        public const string ClaimTypes_Roles = "roles";

        public const string DefaultTimeZone = "UTC";
        public const string DefaultLanguage = "en";
    }
}
