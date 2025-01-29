namespace Domain.Rules
{
    public static class UsernameRules
    {
        public const int MinimumLength = 3;
        public const int MaximumLength = 50;
        public const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
    }
}
