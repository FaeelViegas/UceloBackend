namespace Ucelo.Infrastructure.Security;

public interface IRateLimitService
{
    Task<bool> IsLockedOutAsync(string email);
    Task RecordFailedAttemptAsync(string email);
    Task ResetFailedAttemptsAsync(string email);
}