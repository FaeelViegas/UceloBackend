using System.ComponentModel.DataAnnotations;

namespace Ucelo.Application.DTOs.Auth;

public class AuthenticateRequest
{
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; } = string.Empty;
}