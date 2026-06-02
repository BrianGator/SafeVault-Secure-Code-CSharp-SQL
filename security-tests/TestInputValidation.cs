// Written by Brian McCarthy
using NUnit.Framework;
using SafeVault.Web.Services;

namespace SafeVault.Tests;

[TestFixture]
public class TestInputValidation
{
    [Test]
    public void TestForSQLInjection()
    {
        string attack = "admin'; DROP TABLE Users;--";
        string sanitized = InputValidator.SanitizeUsername(attack);

        Assert.That(sanitized, Does.Not.Contain("'"));
        Assert.That(sanitized, Does.Not.Contain(";"));
        Assert.That(sanitized, Does.Not.Contain("--"));
        Assert.That(sanitized, Does.Not.Contain("DROP"));
    }

    [Test]
    public void TestForXSS()
    {
        string attack = "<script>alert('xss')</script>Brian";
        string encoded = InputValidator.EncodeForHtml(attack);

        Assert.That(encoded, Does.Not.Contain("<script>"));
        Assert.That(encoded, Does.Contain("&lt;script&gt;"));
    }

    [Test]
    public void ValidEmailPassesAndScriptEmailFails()
    {
        Assert.That(InputValidator.IsValidEmail("brian@example.com"), Is.True);
        Assert.That(InputValidator.IsValidEmail("<script>alert(1)</script>@example.com"), Is.False);
    }

    [Test]
    public void UsernameAllowsOnlyExpectedCharacters()
    {
        Assert.That(InputValidator.IsValidUsername("Brian_McCarthy-01"), Is.True);
        Assert.That(InputValidator.IsValidUsername("Brian<script>"), Is.False);
    }
}
