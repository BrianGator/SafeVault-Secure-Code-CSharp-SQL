// Written by Brian McCarthy
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace SafeVault.Web.Services;

public static class InputValidator
{
    private static readonly Regex AllowedUsernameCharacters = new("[^a-zA-Z0-9._-]", RegexOptions.Compiled);

    public static string SanitizeUsername(string? username)
    {
        if (string.IsNullOrWhiteSpace(username)) return string.Empty;

        string trimmed = username.Trim();
        string withoutSqlOrScriptTokens = trimmed
            .Replace("--", string.Empty)
            .Replace(";", string.Empty)
            .Replace("'", string.Empty)
            .Replace("\"", string.Empty)
            .Replace("<", string.Empty)
            .Replace(">", string.Empty);

        string cleaned = AllowedUsernameCharacters.Replace(withoutSqlOrScriptTokens, string.Empty);
        return cleaned.Length > 50 ? cleaned[..50] : cleaned;
    }

    public static bool IsValidUsername(string? username)
    {
        string sanitized = SanitizeUsername(username);
        return sanitized.Length is >= 3 and <= 50 && sanitized == username?.Trim();
    }

    public static string SanitizeEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return string.Empty;

        string cleaned = email.Trim()
            .Replace("<", string.Empty)
            .Replace(">", string.Empty)
            .Replace("\"", string.Empty)
            .Replace("'", string.Empty)
            .Replace(";", string.Empty);

        return cleaned.Length > 100 ? cleaned[..100] : cleaned;
    }

    public static bool IsValidEmail(string? email)
    {
        string sanitized = SanitizeEmail(email);
        if (string.IsNullOrWhiteSpace(sanitized)) return false;

        try
        {
            var address = new MailAddress(sanitized);
            return address.Address.Equals(sanitized, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    public static string EncodeForHtml(string? value)
    {
        return HtmlEncoder.Default.Encode(value ?? string.Empty);
    }
}
