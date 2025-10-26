using Microsoft.Extensions.Options;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;

namespace Ucelo.Infrastructure.Security;

public class RateLimitService : IRateLimitService
{
    private readonly ILoginAttemptRepository _loginAttemptRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RateLimitSettings _settings;

    public RateLimitService(
        ILoginAttemptRepository loginAttemptRepository,
        IUnitOfWork unitOfWork,
        IOptions<RateLimitSettings> settings)
    {
        _loginAttemptRepository = loginAttemptRepository;
        _unitOfWork = unitOfWork;
        _settings = settings.Value;
    }

    public async Task<bool> IsLockedOutAsync(string email)
    {
        var attempt = await _loginAttemptRepository.GetByEmailAsync(email);

        if (attempt == null) return false;

        return attempt.IsLockedOut();
    }

    public async Task RecordFailedAttemptAsync(string email)
    {
        var attempt = await _loginAttemptRepository.GetByEmailAsync(email);

        if (attempt == null)
        {
            attempt = new LoginAttempt(email);
            attempt.RecordFailedAttempt(
                _settings.MaxFailedAttempts,
                _settings.LockoutDurationMinutes,
                _settings.AttemptWindowMinutes
            );

            await _loginAttemptRepository.AddAsync(attempt);
        }
        else
        {
            attempt.RecordFailedAttempt(
                _settings.MaxFailedAttempts,
                _settings.LockoutDurationMinutes,
                _settings.AttemptWindowMinutes
            );

            await _loginAttemptRepository.UpdateAsync(attempt);
        }

        await _unitOfWork.CommitAsync();
    }

    public async Task ResetFailedAttemptsAsync(string email)
    {
        var attempt = await _loginAttemptRepository.GetByEmailAsync(email);

        if (attempt == null) return;

        await _loginAttemptRepository.DeleteAsync(attempt);
        await _unitOfWork.CommitAsync();
    }
}