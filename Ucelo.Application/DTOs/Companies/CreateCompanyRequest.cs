using System.ComponentModel.DataAnnotations;

namespace Ucelo.Application.DTOs.Companies;

public class CreateCompanyRequest
{
    [Required(ErrorMessage = "A razão social é obrigatória")]
    [MaxLength(500, ErrorMessage = "A razão social não pode exceder 500 caracteres")]
    public string LegalName { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "O nome fantasia não pode exceder 500 caracteres")]
    public string? TradeName { get; set; }

    [Required(ErrorMessage = "O CNPJ é obrigatório")]
    [RegularExpression(@"^\d{14}$|^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$", 
        ErrorMessage = "Formato de CNPJ inválido")]
    public string TaxId { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "A inscrição estadual não pode exceder 20 caracteres")]
    public string? StateRegistration { get; set; }

    [Phone(ErrorMessage = "Formato de telefone inválido")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Formato de e-mail corporativo inválido")]
    public string? CorporateEmail { get; set; }
}