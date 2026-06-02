# RBAC - Role-Based Access Control

**Written by Brian McCarthy**

## Purpose

This folder contains the role-based authorization code for SafeVault. RBAC restricts sensitive functionality to users with the proper role.

## Files

- `RoleAuthorizationService.cs` contains role-checking methods.

## What Was Done

1. Users are assigned roles such as `admin` or `user`.
2. Administrative features require the `admin` role.
3. Normal users are denied access to admin-only functionality.
4. Role checks use exact role comparisons and ignore case safely.

## Admin Authorization Example

```csharp
public bool CanAccessAdminDashboard(UserRecord? user) => HasRole(user, "admin");
```

## Result

The application enforces least privilege. Users only receive access appropriate to their role, and administrative actions are protected from unauthorized users.
