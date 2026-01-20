using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Weather.Domain.Entities;
using Weather.Domain.Settings;
using Weather.Application.Common.Interfaces;

namespace Weather.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;
    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _settings = jwtSettings.Value;
    }

    public Dictionary<string, string> CreateToken(User user)
    {
        // Define the user data (claims) to be encoded inside the token
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, user.Name),
            new(ClaimTypes.Role, user.Role.ToString()) // Role added for Authorization attributes
        };

        // Create the cryptographic key and signing credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Define the token metadata (who issued it, how long it lasts)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            SigningCredentials = creds,
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        // Generate the raw token object
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Calculate expiration and return as a serialized string
        var expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);
        
        return new Dictionary<string, string>
        {
            ["token"] = tokenHandler.WriteToken(token), // Serializes to the long JWT string
            ["expiresAt"] = expires.ToString("O")       // ISO 8601 format for frontend parsing
        };
    }
}