# Prevent SQL Injection and XSS Vulnerabilities

**Written by Brian McCarthy**

## Purpose

This folder contains the main SafeVault code used to prevent SQL injection and cross-site scripting, also called XSS.

## What Was Done

1. User input is validated before it is accepted by the application.
2. Usernames are restricted to safe characters: letters, numbers, periods, underscores, and hyphens.
3. Unsafe characters such as `<`, `>`, quotes, semicolons, and SQL comment tokens are removed from form input.
4. Email values are validated with `System.Net.Mail.MailAddress` before being stored.
5. SQL commands use parameterized statements instead of string concatenation.
6. User-controlled output is encoded with `HtmlEncoder.Default.Encode()` before display.

## SQL Injection Prevention

The file `UserRepository-ParameterizedQueries.cs` uses SQL parameters such as `@Username`, `@Email`, `@PasswordHash`, `@Role`, and `@SearchTerm`. These placeholders ensure submitted values are treated as data rather than executable SQL.

Example secure pattern:

```csharp
command.CommandText = """
    SELECT UserID, Username, Email, PasswordHash, Role
    FROM Users
    WHERE Username = @Username;
    """;
command.Parameters.AddWithValue("@Username", username);
```

## XSS Prevention

The file `InputValidator.cs` removes obvious script characters from form input and encodes output before rendering it back into HTML.

Example secure pattern:

```csharp
public static string EncodeForHtml(string? value)
{
    return HtmlEncoder.Default.Encode(value ?? string.Empty);
}
```

## Result

The application blocks common SQL injection payloads and prevents raw script tags from being returned as executable browser content.
