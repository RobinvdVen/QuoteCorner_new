namespace Application.Common.Constants
{
    public static class ValidationConstants
    {
        public static class Password
        {
            public const int MinLength = 8;
            public const int MaxLength = 128;
            public const string Pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        }

        public static class Username
        {
            public const int MinLength = 3;
            public const int MaxLength = 50;
            public const string Pattern = @"^[a-zA-Z0-9_-]*$";
        }
    }
}
