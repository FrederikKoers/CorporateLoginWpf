using CorporateLogin.Common.Models;
using CorporateLogin.Services;
using CorporateLogin.Tests.Helpers;
using System.Security;
using CorporateLogin.Common.Interfaces;
using Xunit;

namespace CorporateLogin.Tests;

public class SecureServiceTests
{
    private readonly ISecureService _secureService;
    private readonly SecureString _validPassword = SecureStringHelper.CreateSecureString("ValidPassword123!");
    private readonly SecureString _invalidPassword = SecureStringHelper.CreateSecureString("InvalidPassword");


    public SecureServiceTests()
    {
        _secureService = new SecureService();
    }

    [Fact]
    public void CreateInitialUser()
    {
        var name = "TestUser";

        var user = _secureService.CreateInitialUser(name, _validPassword);
        
        Assert.NotNull(user);
        Assert.Equal(name, user.Username);
        Assert.True(!string.IsNullOrEmpty(user.PasswordHash));
    }

    [Fact]
    public void CheckPasswordWithValidPassword()
    {
        var user = new User
        {
            Username = "TestUser",
            PasswordHash = _secureService
                .CreateInitialUser("TestUser", _validPassword).PasswordHash
        };

        // Act
        var result = _secureService.CheckPassword(user, _validPassword);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckPasswordWithInvalidPassword()
    {
        var user = new User
        {
            Username = "TestUser",
            PasswordHash = _secureService
                .CreateInitialUser("TestUser", _validPassword).PasswordHash
        };

        var result = _secureService.CheckPassword(user, _invalidPassword);

        Assert.False(result);
    }

    [Fact]
    public void CheckPasswordRules_ValidPassword_ReturnsTrue()
    {
        var result = _secureService.CheckPasswordRules(_validPassword);
        
        Assert.True(result);
    }

    [Fact]
    public void CheckPasswordRulesWithInvalidPassword()
    {
        var result = _secureService.CheckPasswordRules(_invalidPassword);

        Assert.False(result);
    }

    [Fact]
    public void CheckPasswordRulesWithNoDigit()
    {
        var noDigitPassword = SecureStringHelper.CreateSecureString("InvalidPassword!");

        var result = _secureService.CheckPasswordRules(noDigitPassword);

        Assert.False(result);
    }

    [Fact]
    public void CheckPasswordRulesWithNoUpperChar()
    {
        var noUpperCharPassword = SecureStringHelper.CreateSecureString("invalidpassword123!");

        var result = _secureService.CheckPasswordRules(noUpperCharPassword);

        Assert.False(result);
    }

    [Fact]
    public void CheckPasswordRulesWithNoLowerChar()
    {
        var noLowerCharPassword = SecureStringHelper.CreateSecureString("INVALIDPASSWORD123!");

        var result = _secureService.CheckPasswordRules(noLowerCharPassword);

        Assert.False(result);
    }

    [Fact]
    public void CheckPasswordRulesWithNoSpecialChar()
    {
        var noSpecialCharPassword = SecureStringHelper.CreateSecureString("InvalidPassword123");

        var result = _secureService.CheckPasswordRules(noSpecialCharPassword);

        Assert.False(result);
    }
}