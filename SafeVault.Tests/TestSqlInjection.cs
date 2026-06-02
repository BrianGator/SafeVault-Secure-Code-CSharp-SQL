// Written by Brian McCarthy
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using SafeVault.Web.Data;
using SafeVault.Web.Services;

namespace SafeVault.Tests;

[TestFixture]
public class TestSqlInjection
{
    private SqliteConnection _connection = null!;
    private UserRepository _repository = null!;
    private PasswordService _passwordService = null!;

    [SetUp]
    public async Task SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        await _connection.OpenAsync();
        _repository = new UserRepository(_connection.ConnectionString);
        _passwordService = new PasswordService();
    }

    [Test]
    public async Task ParameterizedLookupDoesNotExecuteInjectedSql()
    {
        string dbPath = Path.Combine(Path.GetTempPath(), $"safevault-test-{Guid.NewGuid():N}.db");
        string connectionString = $"Data Source={dbPath}";
        var repository = new UserRepository(connectionString);
        await repository.InitializeAsync();
        await repository.AddUserAsync("admin", "admin@example.com", _passwordService.HashPassword("SecurePass123!"), "admin");

        string maliciousUsername = "admin'; DROP TABLE Users;--";
        var result = await repository.GetUserByUsernameAsync(maliciousUsername);
        var usersAfterAttack = await repository.SearchUsersAsync("admin");

        Assert.That(result, Is.Null);
        Assert.That(usersAfterAttack.Count, Is.EqualTo(1));

        File.Delete(dbPath);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _connection.DisposeAsync();
    }
}
