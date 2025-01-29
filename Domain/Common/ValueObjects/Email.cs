namespace Domain.Common.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Result<Email>.Failure("Email cannot be empty");

            if (email.Length > 256)
                return Result<Email>.Failure("Email cannot exceed 256 characters");

            try
            {
                // Use MailAddress for validating email format
                var mailAddress = new System.Net.Mail.MailAddress(email);

                if (mailAddress.Address != email)
                    return Result<Email>.Failure("Invalid email format");

                return Result<Email>.Success(new Email(email));
            }
            catch
            {
                return Result<Email>.Failure("Invalid email format");
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static bool TryParse(string email, out Email result)
        {
            var createResult = Create(email);
            result = createResult.IsSuccess ? createResult.Value : null;
            return createResult.IsSuccess;
        }
    }
}
