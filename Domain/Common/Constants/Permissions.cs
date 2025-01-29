namespace Domain.Common.Constants
{
    public static class Permissions
    {
        public static class Users
        {
            public const string View = "Users.View";
            public const string Create = "Users.Create";
            public const string Edit = "Users.Edit";
            public const string Delete = "Users.Delete";
            public const string ManageRoles = "Users.ManageRoles";
        }

        public static class Roles
        {
            public const string View = "Roles.View";
            public const string Create = "Roles.Create";
            public const string Edit = "Roles.Edit";
            public const string Delete = "Roles.Delete";
            public const string ManagePermissions = "Roles.ManagePermissions";
        }

        public static class Audit
        {
            public const string View = "Audit.View";
            public const string Export = "Audit.Export";
        }

        public static class System
        {
            public const string ManageSettings = "System.ManageSettings";
            public const string ViewMetrics = "System.ViewMetrics";
        }
    }
}
