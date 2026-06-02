// Written by Brian McCarthy
using Microsoft.Data.Sqlite;
using SafeVault.Web.Models;
using SafeVault.Web.Services;

namespace SafeVault.Web.Data;

public interface IUserRepository
{
    Task InitializeAsync();
    Task AddUserAsync(string username, string email, string passwordHash, string role);
    Task<UserRecord?> GetUserByUsernameAsync(string username);
    Task<IReadOnlyList<UserRecord>> SearchUsersAsync(string searchTerm);
}

public sealed class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Users (
                UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Email TEXT NOT NULL,
                PasswordHash TEXT NOT NULL,
                Role TEXT NOT NULL CHECK(Role IN ('admin', 'user'))
            );
            """;

        await command.ExecuteNonQueryAsync();
    }

    public async Task AddUserAsync(string username, string email, string passwordHash, string role)
    {
        string safeUsername = InputValidator.SanitizeUsername(username);
        string safeEmail = InputValidator.SanitizeEmail(email);

        if (!InputValidator.IsValidUsername(safeUsername)) throw new ArgumentException("Invalid username.", nameof(username));
        if (!InputValidator.IsValidEmail(safeEmail)) throw new ArgumentException("Invalid email.", nameof(email));
        if (role is not ("admin" or "user")) throw new ArgumentException("Role must be admin or user.", nameof(role));

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Users (Username, Email, PasswordHash, Role)
            VALUES (@Username, @Email, @PasswordHash, @Role);
            """;
        command.Parameters.AddWithValue("@Username", safeUsername);
        command.Parameters.AddWithValue("@Email", safeEmail);
        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
        command.Parameters.AddWithValue("@Role", role);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<UserRecord?> GetUserByUsernameAsync(string username)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT UserID, Username, Email, PasswordHash, Role
            FROM Users
            WHERE Username = @Username;
            """;
        command.Parameters.AddWithValue("@Username", username);

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return MapUser(reader);
    }

    public async Task<IReadOnlyList<UserRecord>> SearchUsersAsync(string searchTerm)
    {
        var users = new List<UserRecord>();

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT UserID, Username, Email, PasswordHash, Role
            FROM Users
            WHERE Username LIKE @SearchTerm OR Email LIKE @SearchTerm;
            """;
        command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(MapUser(reader));
        }

        return users;
    }

    private static UserRecord MapUser(SqliteDataReader reader) => new(
        reader.GetInt32(0),
        reader.GetString(1),
        reader.GetString(2),
        reader.GetString(3),
        reader.GetString(4)
    );
}
