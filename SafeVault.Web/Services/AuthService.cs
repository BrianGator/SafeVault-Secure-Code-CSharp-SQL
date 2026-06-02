// Written by Brian McCarthy
using SafeVault.Web.Data;
using SafeVault.Web.Models;

namespace SafeVault.Web.Services;

public interface IAuthService
{
    Task<AuthResult> AuthenticateAsync(string username, string password);
}

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public AuthService(IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<AuthResult> AuthenticateAsync(string username, string password)
    {
        string safeUsername = InputValidator.SanitizeUsername(username);
        if (!InputValidator.IsValidUsername(safeUsername))
        {
            return new AuthResult(false, "Invalid username or password.");
        }

        UserRecord? user = await _userRepository.GetUserByUsernameAsync(safeUsername);
        if (user is null || !_passwordService.VerifyPassword(password, user.PasswordHash))
        {
            return new AuthResult(false, "Invalid username or password.");
        }

        return new AuthResult(true, "Authentication successful.", user);
    }
}
