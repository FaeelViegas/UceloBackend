using Ucelo.Domain.Enums;

namespace Ucelo.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string SaltHash { get; private set; }
    public UserType UserType { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation Properties
    public Individual? Individual { get; private set; }
    public Company? Company { get; private set; }

    private User() { } // EF Core constructor

    public User(
        string email, 
        string passwordHash, 
        string saltHash, 
        UserType userType)
    {
        ValidateEmail(email);
        ValidatePasswordHash(passwordHash);
        ValidateSaltHash(saltHash);

        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        SaltHash = saltHash;
        UserType = userType;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(string newEmail)
    {
        ValidateEmail(newEmail);
        Email = newEmail.ToLowerInvariant();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string newPasswordHash, string newSaltHash)
    {
        ValidatePasswordHash(newPasswordHash);
        ValidateSaltHash(newSaltHash);
        PasswordHash = newPasswordHash;
        SaltHash = newSaltHash;
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

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail é obrigatório");

        if (email.Length > 255)
            throw new ArgumentException("O e-mail não pode exceder 255 caracteres");

        if (!email.Contains("@") || !email.Contains("."))
            throw new ArgumentException("Formato de e-mail inválido");
    }

    private void ValidatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("O hash da senha é obrigatório");

        if (passwordHash.Length > 500)
            throw new ArgumentException("Hash da senha inválido");
    }

    private void ValidateSaltHash(string saltHash)
    {
        if (string.IsNullOrWhiteSpace(saltHash))
            throw new ArgumentException("O salt hash é obrigatório");

        if (saltHash.Length > 500)
            throw new ArgumentException("Salt hash inválido");
    }
}