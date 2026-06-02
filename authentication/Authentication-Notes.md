# Authentication Implementation

**Written by Brian McCarthy**

## Purpose

This folder contains the SafeVault authentication code. Authentication verifies that a user is legitimate before allowing access to protected features.

## Files

- `AuthService.cs` verifies login attempts.
- `PasswordService.cs` hashes passwords and verifies submitted passwords against stored bcrypt hashes.

## What Was Done

1. Passwords are never stored as plain text.
2. Passwords are hashed using `BCrypt.Net-Next`.
3. The bcrypt work factor is set to 12 to make password cracking more difficult.
4. Login checks compare the submitted password against the stored hash.
5. Invalid usernames or passwords return a failed authentication result.

## Secure Password Example

```csharp
public string HashPassword(string password)
{
    return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
}
```

## Login Verification Example

```csharp
return _passwordService.VerifyPassword(password, user.PasswordHash) ? user : null;
```

## Result

SafeVault uses secure password hashing and credential verification instead of plain-text password storage or unsafe comparisons.
