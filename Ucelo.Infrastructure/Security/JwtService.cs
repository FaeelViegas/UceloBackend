using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ucelo.Domain.Entities;

namespace Ucelo.Infrastructure.Security;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly RSA _privateRsa;
    private readonly RSA _publicRsa;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;

        // Importar chave privada (PKCS#8)
        var privateKeyBytes = Convert.FromBase64String(_jwtSettings.PrivateKeyBase64);
        _privateRsa = RSA.Create();
        _privateRsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

        // Importar chave pública (SPKI)
        var publicKeyBytes = Convert.FromBase64String(_jwtSettings.PublicKeyBase64);
        _publicRsa = RSA.Create();
        _publicRsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
    }

    public string GenerateToken(User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("user_type", user.UserType.ToString())
        };

        var credentials = new SigningCredentials(
            new RsaSecurityKey(_privateRsa),
            SecurityAlgorithms.RsaSha256
        );

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new RsaSecurityKey(_publicRsa),
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public DateTime GetTokenExpiration()
    {
        return DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
    }
}