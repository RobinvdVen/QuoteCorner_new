namespace Domain.Common.Constants
{
    public static class UserConstants
    {
        public const int MaxFailedAttempts = 5;
        public const int LockoutDurationMinutes = 30;
        public const int MinimumPasswordLength = 8;
        public const int MaximumPasswordLength = 128;
        public const int MinimumUsernameLength = 3;
        public const int MaximumUsernameLength = 50;
        public const int PasswordResetTokenExpiryHours = 24;
    }
}
