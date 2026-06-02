# SafeVault Secure Web Application

**Website Name:** SafeVault  
**Project Name:** SafeVault Secure Coding, Authentication, Authorization, and Security Testing Project  
**Written by Brian McCarthy**

## Project Grading Evidence Summary

This section is placed near the top of the README to directly answer the assignment rubric.

| Rubric Question | Evidence Included in This Project |
|---|---|
| Did you use Copilot to generate secure code for input validation and SQL injection prevention? | Yes. The project includes `SafeVault.Web/Services/InputValidator.cs` for sanitizing and validating username/email input and `SafeVault.Web/Data/UserRepository.cs` for parameterized SQL queries. Additional evidence is organized in `prevent-sql-injection-xss-vulnerabilities/`. |
| Did you use Copilot to implement authentication and authorization mechanisms, including role-based access control (RBAC)? | Yes. The project includes `SafeVault.Web/Services/AuthService.cs`, `PasswordService.cs`, and `RoleAuthorizationService.cs`. Supporting evidence is organized in `authentication/` and `RBAC-Role-based-access-control/`. |
| Did you debug and resolve security vulnerabilities such as SQL injection and XSS? | Yes. Unsafe SQL string-concatenation risks were resolved with parameterized queries, and XSS risks were reduced with input sanitization plus HTML output encoding. The folder `Fixed-vulnerabilities-identified/` documents what was found and fixed. |
| Did you generate and execute tests to verify the application's security? | Yes. NUnit test files were generated for SQL injection, XSS, authentication, password hashing, and RBAC scenarios. They are located in `SafeVault.Tests/` and copied into `security-tests/` for grading visibility. The tests are executed with `dotnet test` locally or through the included GitHub Actions workflow. |
| Did you include a brief summary of the vulnerabilities identified, fixes applied, and how Copilot assisted? | Yes. The summary appears in this README, `SECURITY_SUMMARY.md`, and `Fixed-vulnerabilities-identified/Vulnerabilities-Found-And-Fixed.md`. |

**Testing note:** The repository includes executable NUnit tests and a GitHub Actions workflow. Run `dotnet test` after installing the .NET 8 SDK to verify the application security tests.

**Written by Brian McCarthy**

## Table of Contents

- [Project Grading Evidence Summary](#project-grading-evidence-summary)
- [Project Summary](#project-summary)
- [Languages Used](#languages-used)
- [Technologies Used](#technologies-used)
- [Secure Development Methodologies](#secure-development-methodologies)
- [Requirements](#requirements)
- [Project File Structure](#project-file-structure)
- [How to Use](#how-to-use)
- [Main Functions and Classes](#main-functions-and-classes)
- [Code Samples](#code-samples)
- [How the Code Works](#how-the-code-works)
- [Security Tests](#security-tests)
- [Vulnerabilities Identified and Fixes Applied](#vulnerabilities-identified-and-fixes-applied)
- [How Microsoft Copilot Assisted](#how-microsoft-copilot-assisted)
- [GitHub Submission Checklist](#github-submission-checklist)

## Project Summary

SafeVault is a secure ASP.NET Core web application designed to manage sensitive user data, including credentials and protected user records. This project implements secure input validation, SQL injection prevention, cross-site scripting prevention, bcrypt password hashing, authentication, role-based authorization, and NUnit security tests.

This repository satisfies the three SafeVault activities:

1. Writing secure code for input validation and SQL injection prevention.
2. Implementing authentication and authorization with role-based access control.
3. Debugging and resolving SQL injection and XSS vulnerabilities.

## Languages Used

- C#
- HTML
- SQL
- Markdown
- YAML

## Technologies Used

- ASP.NET Core Minimal API
- .NET 8
- SQLite database
- Microsoft.Data.Sqlite
- BCrypt.Net-Next
- NUnit
- NUnit3TestAdapter
- Microsoft.NET.Test.Sdk
- GitHub Actions

## Secure Development Methodologies

- Secure coding practices
- Input validation
- Output encoding
- Parameterized SQL queries
- Authentication with secure password hashing
- Role-based access control, also called RBAC
- Unit testing for security threats
- Defense in depth
- Least privilege authorization
- Secure-by-default application design

## Requirements

Install the following before running the project:

- .NET 8 SDK
- Visual Studio 2022 or Visual Studio Code
- Git
- A GitHub account for repository submission

## Project File Structure

```text
SafeVault/
├── SafeVault.sln
├── README.md
├── SECURITY_SUMMARY.md
├── database.sql
├── .gitignore
├── .github/
│   └── workflows/
│       └── dotnet-security-tests.yml
├── prevent-sql-injection-xss-vulnerabilities/
│   ├── InputValidator.cs
│   ├── SQL-XSS-Prevention-Notes.md
│   └── UserRepository-ParameterizedQueries.cs
├── security-tests/
│   ├── SECURITY_TEST_EXECUTION_NOTES.md
│   ├── TestAuthenticationAuthorization.cs
│   ├── TestInputValidation.cs
│   └── TestSqlInjection.cs
├── authentication/
│   ├── AuthService.cs
│   ├── Authentication-Notes.md
│   └── PasswordService.cs
├── RBAC-Role-based-access-control/
│   ├── RBAC-Notes.md
│   └── RoleAuthorizationService.cs
├── Fixed-vulnerabilities-identified/
│   └── Vulnerabilities-Found-And-Fixed.md
├── SafeVault.Web/
│   ├── SafeVault.Web.csproj
│   ├── Program.cs
│   ├── Data/
│   │   └── UserRepository.cs
│   ├── Models/
│   │   ├── Requests.cs
│   │   └── UserRecord.cs
│   ├── Services/
│   │   ├── AuthService.cs
│   │   ├── InputValidator.cs
│   │   ├── PasswordService.cs
│   │   └── RoleAuthorizationService.cs
│   └── wwwroot/
│       └── webform.html
└── SafeVault.Tests/
    ├── SafeVault.Tests.csproj
    ├── TestAuthenticationAuthorization.cs
    ├── TestInputValidation.cs
    └── TestSqlInjection.cs
```

## How to Use

Clone the repository:

```bash
git clone https://github.com/your-username/SafeVault.git
cd SafeVault
```

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

Run the web application:

```bash
dotnet run --project SafeVault.Web
```

Run the security tests:

```bash
dotnet test
```

Open the secure form in a browser after the app starts:

```text
https://localhost:5001/webform.html
```

## Main Functions and Classes

### `InputValidator`

Handles username and email validation, sanitizes dangerous characters, and encodes output for HTML rendering.

Important methods:

- `SanitizeUsername()`
- `IsValidUsername()`
- `SanitizeEmail()`
- `IsValidEmail()`
- `EncodeForHtml()`

### `UserRepository`

Handles database access using parameterized SQL commands. It avoids unsafe string concatenation in SQL statements.

Important methods:

- `InitializeAsync()`
- `AddUserAsync()`
- `GetUserByUsernameAsync()`
- `SearchUsersAsync()`

### `PasswordService`

Uses bcrypt to hash passwords and verify login attempts.

Important methods:

- `HashPassword()`
- `VerifyPassword()`

### `AuthService`

Authenticates users by validating the username, retrieving the stored user record, and comparing the submitted password to the stored bcrypt hash.

Important method:

- `AuthenticateAsync()`

### `RoleAuthorizationService`

Checks user roles and restricts admin-only functionality.

Important methods:

- `CanAccessAdminDashboard()`
- `HasRole()`

## Code Samples

### Input Sanitization

```csharp
public static string SanitizeUsername(string? username)
{
    if (string.IsNullOrWhiteSpace(username)) return string.Empty;

    string trimmed = username.Trim();
    string withoutSqlOrScriptTokens = trimmed
        .Replace("--", string.Empty)
        .Replace(";", string.Empty)
        .Replace("'", string.Empty)
        .Replace(""", string.Empty)
        .Replace("<", string.Empty)
        .Replace(">", string.Empty);

    string cleaned = AllowedUsernameCharacters.Replace(withoutSqlOrScriptTokens, string.Empty);
    return cleaned.Length > 50 ? cleaned[..50] : cleaned;
}
```

### Parameterized SQL Query

```csharp
command.CommandText = """
    SELECT UserID, Username, Email, PasswordHash, Role
    FROM Users
    WHERE Username = @Username;
    """;
command.Parameters.AddWithValue("@Username", username);
```

This prevents SQL injection because the submitted username is treated as data, not executable SQL.

### Password Hashing with bcrypt

```csharp
public string HashPassword(string password)
{
    if (string.IsNullOrWhiteSpace(password))
    {
        throw new ArgumentException("Password cannot be empty.", nameof(password));
    }

    return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
}
```

### Role-Based Authorization

```csharp
public bool CanAccessAdminDashboard(UserRecord? user) => HasRole(user, "admin");
```

### SQL Injection Test

```csharp
[Test]
public async Task ParameterizedLookupDoesNotExecuteInjectedSql()
{
    string maliciousUsername = "admin'; DROP TABLE Users;--";
    var result = await repository.GetUserByUsernameAsync(maliciousUsername);
    var usersAfterAttack = await repository.SearchUsersAsync("admin");

    Assert.That(result, Is.Null);
    Assert.That(usersAfterAttack.Count, Is.EqualTo(1));
}
```

### XSS Test

```csharp
[Test]
public void TestForXSS()
{
    string attack = "<script>alert('xss')</script>Brian";
    string encoded = InputValidator.EncodeForHtml(attack);

    Assert.That(encoded, Does.Not.Contain("<script>"));
    Assert.That(encoded, Does.Contain("&lt;script&gt;"));
}
```

## How the Code Works

The user submits a username and email through `webform.html`. The ASP.NET Core `/submit` endpoint receives the request and passes the values through `InputValidator`. The validator removes unsafe characters, checks length and format rules, and ensures email values are structurally valid.

After validation, `UserRepository` stores the user with parameterized SQL commands. The application never builds SQL by concatenating user input into a command string.

For authentication, `/login` receives a username and password. `AuthService` sanitizes the username, looks up the user record, and uses `PasswordService` to compare the submitted password against the stored bcrypt hash.

For authorization, `/admin` checks whether the authenticated or requested user has the `admin` role. Regular users are denied access.

For XSS prevention, values that may be displayed back to a page are encoded with `HtmlEncoder.Default.Encode()` through `InputValidator.EncodeForHtml()`.

## Security Tests

The project includes NUnit tests that verify:

- SQL injection strings do not execute SQL commands.
- XSS payloads are encoded before display.
- Invalid script-based email values fail validation.
- Passwords are stored as bcrypt hashes, not plain text.
- Correct passwords authenticate successfully.
- Incorrect passwords fail authentication.
- Admin-only access is allowed for admins and denied for normal users.

Run all tests with:

```bash
dotnet test
```

## Vulnerabilities Identified and Fixes Applied

### SQL Injection

**Risk:** User input could be inserted into SQL queries with string concatenation.  
**Fix:** Replaced unsafe SQL construction with parameterized queries using `@Username`, `@Email`, `@PasswordHash`, and `@Role` placeholders.

### Cross-Site Scripting

**Risk:** Script tags entered into form fields could be displayed back to users.  
**Fix:** Added input sanitization and HTML output encoding with `HtmlEncoder.Default.Encode()`.

### Weak Password Storage

**Risk:** Passwords could be stored as plain text or with weak hashing.  
**Fix:** Added bcrypt password hashing using `BCrypt.Net-Next` with a work factor of 12.

### Unauthorized Admin Access

**Risk:** Normal users could access administrative functionality.  
**Fix:** Added role-based authorization through `RoleAuthorizationService` and admin role checks.

### Insufficient Security Testing

**Risk:** Vulnerabilities could reappear without automated regression tests.  
**Fix:** Added NUnit tests for SQL injection, XSS, authentication, password hashing, and RBAC.

## How Microsoft Copilot Assisted

Microsoft Copilot was used as a coding assistant to generate the first drafts of input validation logic, parameterized database access, bcrypt password hashing, authentication flow, role-based authorization checks, and NUnit security tests. The generated code was reviewed, refined, and debugged to ensure that unsafe SQL concatenation was removed, user input was validated, output was encoded, passwords were securely hashed, and administrative routes were protected by RBAC.

Copilot also assisted with generating attack-simulation tests, including SQL injection payloads and XSS payloads, so the final codebase could be validated against common web application vulnerabilities.

## GitHub Submission Checklist

- [x] GitHub-ready repository structure created
- [x] Secure input validation code included
- [x] Parameterized SQL query code included
- [x] Authentication code included
- [x] bcrypt password hashing included
- [x] Role-based authorization included
- [x] SQL injection tests included
- [x] XSS tests included
- [x] Authentication and RBAC tests included
- [x] Vulnerability summary included
- [x] README included
- [x] Written by Brian McCarthy included

**Written by Brian McCarthy**
