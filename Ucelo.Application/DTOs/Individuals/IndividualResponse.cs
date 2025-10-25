namespace Ucelo.Application.DTOs.Individuals;

public class IndividualResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? IdentityCard { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Phone { get; set; }
    public string? MobilePhone { get; set; }
    public DateTime CreatedAt { get; set; }
}