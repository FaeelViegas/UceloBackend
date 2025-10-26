namespace Ucelo.Application.DTOs.Auth;

public class AuthenticateResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public DateTime ExpiresAt { get; set; }
    public string UserType { get; set; } = string.Empty;
}