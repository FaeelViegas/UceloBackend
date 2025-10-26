using Ucelo.Application.DTOs.Auth;

namespace Ucelo.Application.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request);
}