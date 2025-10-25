namespace Ucelo.Domain.Entities;

public class Company
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string LegalName { get; private set; }
    public string? TradeName { get; private set; }
    public string TaxId { get; private set; } // CNPJ
    public string? StateRegistration { get; private set; }
    public string? Phone { get; private set; }
    public string? CorporateEmail { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public User User { get; private set; }

    private Company() { } // EF Core constructor

    public Company(
        int userId,
        string legalName,
        string taxId,
        string? tradeName = null,
        string? stateRegistration = null,
        string? phone = null,
        string? corporateEmail = null)
    {
        ValidateLegalName(legalName);
        ValidateTaxId(taxId);

        UserId = userId;
        LegalName = legalName;
        TradeName = tradeName;
        TaxId = CleanTaxId(taxId);
        StateRegistration = stateRegistration;
        Phone = phone;
        CorporateEmail = corporateEmail;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string legalName,
        string? tradeName = null,
        string? stateRegistration = null,
        string? phone = null,
        string? corporateEmail = null)
    {
        ValidateLegalName(legalName);

        LegalName = legalName;
        TradeName = tradeName;
        StateRegistration = stateRegistration;
        Phone = phone;
        CorporateEmail = corporateEmail;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateLegalName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A razão social é obrigatória");

        if (name.Length > 500)
            throw new ArgumentException("A razão social não pode exceder 500 caracteres");
    }

    private void ValidateTaxId(string taxId)
    {
        var cleanTaxId = CleanTaxId(taxId);

        if (cleanTaxId.Length != 14)
            throw new ArgumentException("Formato de CNPJ inválido");

        if (!ValidateTaxIdDigits(cleanTaxId))
            throw new ArgumentException("CNPJ inválido");
    }

    private string CleanTaxId(string taxId)
    {
        return new string(taxId.Where(char.IsDigit).ToArray());
    }

    private bool ValidateTaxIdDigits(string taxId)
    {
        // Validação de dígitos verificadores do CNPJ
        if (taxId.Distinct().Count() == 1) return false;

        var multiplier1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempTaxId = taxId.Substring(0, 12);
        var sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempTaxId[i].ToString()) * multiplier1[i];

        var remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        var digit = remainder.ToString();
        tempTaxId += digit;
        sum = 0;

        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempTaxId[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;
        digit += remainder.ToString();

        return taxId.EndsWith(digit);
    }
}