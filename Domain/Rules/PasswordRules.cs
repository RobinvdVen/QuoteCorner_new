namespace Domain.Rules
{
    public static class PasswordRules
    {
        public const int MinimumLength = 8;
        public const int MaximumLength = 128;
        public const bool RequireDigit = true;
        public const bool RequireLowercase = true;
        public const bool RequireUppercase = true;
        public const bool RequireNonAlphanumeric = true;
    }
}
