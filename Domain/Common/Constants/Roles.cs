namespace Domain.Common.Constants
{
    public static class DefaultRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string User = "User";

        public static IEnumerable<string> GetAll()
        {
            yield return SuperAdmin;
            yield return User;
        }
    }
}
