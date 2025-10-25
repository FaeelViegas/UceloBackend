namespace Ucelo.Domain.Entities;

public class Individual
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string FullName { get; private set; }
    public string TaxId { get; private set; } // CPF
    public string? IdentityCard { get; private set; } // RG
    public DateTime? BirthDate { get; private set; }
    public string? Phone { get; private set; }
    public string? MobilePhone { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public User User { get; private set; }

    private Individual() { } // EF Core constructor

    public Individual(
        int userId,
        string fullName,
        string taxId,
        string? identityCard = null,
        DateTime? birthDate = null,
        string? phone = null,
        string? mobilePhone = null)
    {
        ValidateFullName(fullName);
        ValidateTaxId(taxId);

        UserId = userId;
        FullName = fullName;
        TaxId = CleanTaxId(taxId);
        IdentityCard = identityCard;
        BirthDate = birthDate;
        Phone = phone;
        MobilePhone = mobilePhone;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string fullName,
        string? identityCard = null,
        DateTime? birthDate = null,
        string? phone = null,
        string? mobilePhone = null)
    {
        ValidateFullName(fullName);

        FullName = fullName;
        IdentityCard = identityCard;
        BirthDate = birthDate;
        Phone = phone;
        MobilePhone = mobilePhone;
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateFullName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome completo é obrigatório");

        if (name.Length > 100)
            throw new ArgumentException("O nome completo não pode exceder 100 caracteres");
    }

    private void ValidateTaxId(string taxId)
    {
        var cleanTaxId = CleanTaxId(taxId);

        if (cleanTaxId.Length != 11)
            throw new ArgumentException("Formato de CPF inválido");

        if (!ValidateTaxIdDigits(cleanTaxId))
            throw new ArgumentException("CPF inválido");
    }

    private string CleanTaxId(string taxId)
    {
        return new string(taxId.Where(char.IsDigit).ToArray());
    }

    private bool ValidateTaxIdDigits(string taxId)
    {
        // Validação de dígitos verificadores do CPF
        if (taxId.Distinct().Count() == 1) return false;

        var multiplier1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempTaxId = taxId.Substring(0, 9);
        var sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempTaxId[i].ToString()) * multiplier1[i];

        var remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        var digit = remainder.ToString();
        tempTaxId += digit;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempTaxId[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;
        digit += remainder.ToString();

        return taxId.EndsWith(digit);
    }
}