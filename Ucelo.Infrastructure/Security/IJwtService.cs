using System.Security.Claims;
using Ucelo.Domain.Entities;

namespace Ucelo.Infrastructure.Security;

public interface IJwtService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
    DateTime GetTokenExpiration();
}