// Written by Brian McCarthy
using System.ComponentModel.DataAnnotations;

namespace SafeVault.Web.Models;

public sealed record SubmissionRequest(
    [property: Required] string Username,
    [property: Required, EmailAddress] string Email
);

public sealed record LoginRequest(
    [property: Required] string Username,
    [property: Required] string Password
);

public sealed record AuthResult(bool Success, string Message, UserRecord? User = null);
