# Security Tests

**Written by Brian McCarthy**

## Purpose

This folder contains NUnit tests that verify SafeVault resists common security threats.

## Tests Included

- `TestInputValidation.cs` verifies username sanitization, email validation, and XSS output encoding.
- `TestSqlInjection.cs` verifies malicious SQL payloads do not execute and do not destroy database records.
- `TestAuthenticationAuthorization.cs` verifies bcrypt password hashing, valid login, invalid login, and RBAC admin access checks.

## How to Execute

Run the following command from the repository root:

```bash
dotnet test
```

The repository also includes `.github/workflows/dotnet-security-tests.yml`, which runs restore, build, and test steps in GitHub Actions when code is pushed to GitHub.

## Attack Scenarios Covered

| Attack Scenario | Test Coverage | Expected Result |
|---|---|---|
| SQL injection with `admin'; DROP TABLE Users;--` | `TestSqlInjection.cs` | Payload is treated as data and does not drop the table. |
| XSS with `<script>alert('xss')</script>` | `TestInputValidation.cs` | Script is encoded or removed before browser display. |
| Invalid email script payload | `TestInputValidation.cs` | Input fails validation. |
| Incorrect password attempt | `TestAuthenticationAuthorization.cs` | Authentication fails. |
| Normal user attempting admin access | `TestAuthenticationAuthorization.cs` | Access is denied. |

## Execution Note

These tests are executable with the .NET 8 SDK. The project is set up so the tests can be executed locally with `dotnet test` and automatically in GitHub Actions.
