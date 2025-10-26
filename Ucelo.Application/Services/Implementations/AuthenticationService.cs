using Ucelo.Application.DTOs.Auth;
using Ucelo.Application.Services.Interfaces;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Security;

namespace Ucelo.Application.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IRateLimitService _rateLimitService;

    public AuthenticationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IRateLimitService rateLimitService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _rateLimitService = rateLimitService;
    }

    public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request)
    {
        if (await _rateLimitService.IsLockedOutAsync(request.Email))
        {
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }

        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !user.IsActive)
        {
            await _rateLimitService.RecordFailedAttemptAsync(request.Email);
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }

        var isValidPassword = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash,
            user.SaltHash
        );

        if (!isValidPassword)
        {
            await _rateLimitService.RecordFailedAttemptAsync(request.Email);
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }

        await _rateLimitService.ResetFailedAttemptsAsync(request.Email);

        var token = _jwtService.GenerateToken(user);
        var expiration = _jwtService.GetTokenExpiration();

        return new AuthenticateResponse
        {
            AccessToken = token,
            ExpiresAt = expiration,
            UserType = user.UserType.ToString()
        };
    }
}