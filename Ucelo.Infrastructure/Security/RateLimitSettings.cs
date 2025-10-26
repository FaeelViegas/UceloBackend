namespace Ucelo.Infrastructure.Security;

public class RateLimitSettings
{
    public int MaxFailedAttempts { get; set; }
    public int LockoutDurationMinutes { get; set; }
    public int AttemptWindowMinutes { get; set; }
}