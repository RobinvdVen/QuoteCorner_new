namespace Domain.Enums
{
    public enum AuditActionType
    {
        Login,
        Logout,
        PasswordChange,
        PasswordReset,
        ProfileUpdate,
        RoleAssigned,
        RoleRemoved,
        PermissionGranted,
        PermissionRevoked,
        AccountLocked,
        AccountUnlocked
    }
}
