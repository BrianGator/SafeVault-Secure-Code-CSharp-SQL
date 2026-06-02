// Written by Brian McCarthy
using SafeVault.Web.Models;

namespace SafeVault.Web.Services;

public interface IRoleAuthorizationService
{
    bool CanAccessAdminDashboard(UserRecord? user);
    bool HasRole(UserRecord? user, string requiredRole);
}

public sealed class RoleAuthorizationService : IRoleAuthorizationService
{
    public bool CanAccessAdminDashboard(UserRecord? user) => HasRole(user, "admin");

    public bool HasRole(UserRecord? user, string requiredRole)
    {
        if (user is null || string.IsNullOrWhiteSpace(requiredRole)) return false;
        return user.Role.Equals(requiredRole, StringComparison.OrdinalIgnoreCase);
    }
}
