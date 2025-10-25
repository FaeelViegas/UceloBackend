using System.ComponentModel.DataAnnotations;

namespace Ucelo.Application.DTOs.Users;

public class CreateUserRequest
{
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
    [MaxLength(255, ErrorMessage = "O e-mail não pode exceder 255 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    [MaxLength(100, ErrorMessage = "A senha não pode exceder 100 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
    public string UserType { get; set; } = "Common";
}