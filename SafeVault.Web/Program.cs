// Written by Brian McCarthy
using SafeVault.Web.Data;
using SafeVault.Web.Models;
using SafeVault.Web.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("SafeVaultDb") ?? "Data Source=safevault.db";

builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<IRoleAuthorizationService, RoleAuthorizationService>();
builder.Services.AddScoped<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    await repository.InitializeAsync();
}

app.MapPost("/submit", async (SubmissionRequest request, IUserRepository repository, IPasswordService passwordService) =>
{
    string username = InputValidator.SanitizeUsername(request.Username);
    string email = InputValidator.SanitizeEmail(request.Email);

    if (!InputValidator.IsValidUsername(username) || !InputValidator.IsValidEmail(email))
    {
        return Results.BadRequest(new { error = "Invalid username or email." });
    }

    string temporaryHash = passwordService.HashPassword(Guid.NewGuid().ToString("N"));
    await repository.AddUserAsync(username, email, temporaryHash, "user");

    return Results.Ok(new
    {
        message = "User saved securely.",
        username = InputValidator.EncodeForHtml(username),
        email = InputValidator.EncodeForHtml(email)
    });
});

app.MapPost("/login", async (LoginRequest request, IAuthService authService) =>
{
    AuthResult result = await authService.AuthenticateAsync(request.Username, request.Password);
    return result.Success
        ? Results.Ok(new { message = result.Message, username = result.User!.Username, role = result.User.Role })
        : Results.Unauthorized();
});

app.MapGet("/admin", (string username, IUserRepository repository, IRoleAuthorizationService authorization) =>
{
    UserRecord? user = repository.GetUserByUsernameAsync(InputValidator.SanitizeUsername(username)).GetAwaiter().GetResult();
    if (!authorization.CanAccessAdminDashboard(user))
    {
        return Results.Forbid();
    }

    return Results.Ok(new { message = "Welcome to the SafeVault Admin Dashboard." });
});

app.Run();
