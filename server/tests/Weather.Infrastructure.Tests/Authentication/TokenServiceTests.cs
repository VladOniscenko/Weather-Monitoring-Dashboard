using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using Weather.Domain.Entities;
using Weather.Domain.Enums;
using Weather.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Weather.Domain.Settings;

namespace Weather.Infrastructure.Tests.Authentication;

[TestClass]
public class TokenServiceTests
{
    private TokenService _tokenService;

    [TestInitialize]
    public void Setup()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "super_secret_key_must_be_very_long_123!",
            ExpiryMinutes = 60,
            Issuer = "WeatherApp",
            Audience = "WeatherClient"
        };

        IOptions<JwtSettings> options = Options.Create(jwtSettings);
        _tokenService = new TokenService(options);
    }

    [TestMethod]
    public void GenerateToken_WithValidUser_ShouldReturnString()
    {
        // Arrange
        var user = new User("Test User", "test@test.com", "hash", UserRole.Admin);

        // Act
        var createTokenResponse = _tokenService.CreateToken(user);

        // Assert
        Assert.IsNotNull(createTokenResponse);
        Assert.IsTrue(createTokenResponse["token"].Length > 0);
    }

    [TestMethod]
    public void GenerateToken_ShouldContainCorrectClaims()
    {
        // Arrange
        var user = new User("Admin User", "admin@test.com", "hash", UserRole.Admin);

        // Act
        var tokenString = _tokenService.CreateToken(user)["token"];

        // Decode the token to check inside it
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);
        var claims = token.Claims.ToList();

        // Assert
        Assert.AreEqual(user.Id.ToString(), claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
        Assert.AreEqual("Admin", claims.First(c => c.Type == "role").Value);
        Assert.AreEqual("WeatherApp", token.Issuer);
    }
}