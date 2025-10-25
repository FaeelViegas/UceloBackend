using System.ComponentModel.DataAnnotations;

namespace Ucelo.Application.DTOs.Individuals;

public class CreateIndividualRequest
{
    [Required(ErrorMessage = "O nome completo é obrigatório")]
    [MaxLength(100, ErrorMessage = "O nome completo não pode exceder 100 caracteres")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório")]
    [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", 
        ErrorMessage = "Formato de CPF inválido")]
    public string TaxId { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "O RG não pode exceder 20 caracteres")]
    public string? IdentityCard { get; set; }

    public DateTime? BirthDate { get; set; }

    [Phone(ErrorMessage = "Formato de telefone inválido")]
    public string? Phone { get; set; }

    [Phone(ErrorMessage = "Formato de telefone celular inválido")]
    public string? MobilePhone { get; set; }
}