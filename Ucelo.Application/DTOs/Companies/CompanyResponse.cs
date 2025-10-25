namespace Ucelo.Application.DTOs.Companies;

public class CompanyResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string? TradeName { get; set; }
    public string TaxId { get; set; } = string.Empty;
    public string? StateRegistration { get; set; }
    public string? Phone { get; set; }
    public string? CorporateEmail { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}