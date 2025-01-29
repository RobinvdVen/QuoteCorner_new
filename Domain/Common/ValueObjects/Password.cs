using Domain.Common.Constants;

namespace Domain.Common.ValueObjects
{
    public class Password : ValueObject
    {
        public string Value { get; }

        private Password(string value)
        {
            Value = value;
        }

        public static Result<Password> Create(string password)
        {
            if (string.IsNullOrEmpty(password))
                return Result<Password>.Failure("Password cannot be empty");

            if (password.Length < UserConstants.MinimumPasswordLength)
                return Result<Password>.Failure($"Password must be at least {UserConstants.MinimumPasswordLength} characters");

            if (password.Length > UserConstants.MaximumPasswordLength)
                return Result<Password>.Failure($"Password cannot exceed {UserConstants.MaximumPasswordLength} characters");

            // Using pattern matching to ensure it meets complexity requirements
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (!hasUpperCase || !hasLowerCase || !hasDigit || !hasSpecialChar)
                return Result<Password>.Failure("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character");

            return Result<Password>.Success(new Password(password));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
