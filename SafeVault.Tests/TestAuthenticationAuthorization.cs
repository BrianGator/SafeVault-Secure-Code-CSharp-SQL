// Written by Brian McCarthy
using NUnit.Framework;
using SafeVault.Web.Data;
using SafeVault.Web.Models;
using SafeVault.Web.Services;

namespace SafeVault.Tests;

[TestFixture]
public class TestAuthenticationAuthorization
{
    [Test]
    public void PasswordsAreHashedAndVerifiedWithBcrypt()
    {
        var passwordService = new PasswordService();
        string hash = passwordService.HashPassword("SecurePass123!");

        Assert.That(hash, Is.Not.EqualTo("SecurePass123!"));
        Assert.That(passwordService.VerifyPassword("SecurePass123!", hash), Is.True);
        Assert.That(passwordService.VerifyPassword("WrongPassword", hash), Is.False);
    }

    [Test]
    public async Task ValidLoginSucceedsAndInvalidLoginFails()
    {
        string dbPath = Path.Combine(Path.GetTempPath(), $"safevault-auth-{Guid.NewGuid():N}.db");
        string connectionString = $"Data Source={dbPath}";
        var repository = new UserRepository(connectionString);
        var passwordService = new PasswordService();
        var authService = new AuthService(repository, passwordService);

        await repository.InitializeAsync();
        await repository.AddUserAsync("brian", "brian@example.com", passwordService.HashPassword("SecurePass123!"), "user");

        AuthResult success = await authService.AuthenticateAsync("brian", "SecurePass123!");
        AuthResult failure = await authService.AuthenticateAsync("brian", "bad-password");

        Assert.That(success.Success, Is.True);
        Assert.That(failure.Success, Is.False);

        File.Delete(dbPath);
    }

    [Test]
    public void AdminDashboardRequiresAdminRole()
    {
        var authorization = new RoleAuthorizationService();
        var admin = new UserRecord(1, "admin", "admin@example.com", "hash", "admin");
        var regularUser = new UserRecord(2, "brian", "brian@example.com", "hash", "user");

        Assert.That(authorization.CanAccessAdminDashboard(admin), Is.True);
        Assert.That(authorization.CanAccessAdminDashboard(regularUser), Is.False);
        Assert.That(authorization.CanAccessAdminDashboard(null), Is.False);
    }
}
