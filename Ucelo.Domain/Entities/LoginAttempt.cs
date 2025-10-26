namespace Ucelo.Domain.Entities;

public class LoginAttempt
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public int Attempts { get; private set; }
    public DateTime? LockedUntil { get; private set; }
    public DateTime FirstAttempt { get; private set; }
    public DateTime LastAttempt { get; private set; }

    private LoginAttempt()
    {
    } // EF Core

    public LoginAttempt(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        Email = email.ToLowerInvariant();
        Attempts = 0;
        FirstAttempt = DateTime.UtcNow;
        LastAttempt = DateTime.UtcNow;
    }

    public void RecordFailedAttempt(int maxAttempts, int lockoutMinutes, int windowMinutes)
    {
        var now = DateTime.UtcNow;

        // Reset se passou da janela de tentativas
        if ((now - FirstAttempt).TotalMinutes > windowMinutes)
        {
            Attempts = 1;
            FirstAttempt = now;
            LockedUntil = null;
        }
        else
        {
            Attempts++;
        }

        LastAttempt = now;

        if (Attempts >= maxAttempts)
        {
            LockedUntil = now.AddMinutes(lockoutMinutes);
        }
    }

    public void Reset()
    {
        Attempts = 0;
        LockedUntil = null;
        FirstAttempt = DateTime.UtcNow;
        LastAttempt = DateTime.UtcNow;
    }

    public bool IsLockedOut()
    {
        if (LockedUntil == null) return false;

        if (DateTime.UtcNow < LockedUntil.Value)
            return true;

        // Bloqueio expirou, limpar
        LockedUntil = null;
        Attempts = 0;
        return false;
    }
}