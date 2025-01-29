using Domain.Common.Constants;
using System.Text.RegularExpressions;

namespace Domain.Common.ValueObjects
{
    public class Username : ValueObject
    {
        public string Value { get; }

        private Username(string value)
        {
            Value = value;
        }

        public static Result<Username> Create(string username)
        {
            if (string.IsNullOrEmpty(username))
                return Result<Username>.Failure("Username cannot be empty");

            if (username.Length < UserConstants.MinimumUsernameLength)
                return Result<Username>.Failure($"Username must be at least {UserConstants.MinimumUsernameLength} characters");

            if (username.Length > UserConstants.MaximumUsernameLength)
                return Result<Username>.Failure($"Username cannot exceed {UserConstants.MaximumUsernameLength} characters");

            // Username can only contain letters, numbers, underscore and dash
            if (!Regex.IsMatch(username, "^[a-zA-Z0-9_-]*$"))
                return Result<Username>.Failure("Username can only contain letters, numbers, underscore and dash");

            return Result<Username>.Success(new Username(username));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
