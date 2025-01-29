namespace Application.Common.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base("Authentication failed.") { }

        public AuthenticationException(string message) : base(message) { }
    }
}
