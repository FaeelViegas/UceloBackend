using Ucelo.Domain.Entities;

namespace Ucelo.Domain.Interfaces;

public interface ILoginAttemptRepository
{
    Task<LoginAttempt?> GetByEmailAsync(string email);
    Task AddAsync(LoginAttempt attempt);
    Task UpdateAsync(LoginAttempt attempt);
    Task DeleteAsync(LoginAttempt attempt);
}