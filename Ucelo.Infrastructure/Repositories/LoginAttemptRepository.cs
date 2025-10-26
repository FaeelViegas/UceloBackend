using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;

namespace Ucelo.Infrastructure.Repositories;

public class LoginAttemptRepository : ILoginAttemptRepository
{
    private readonly UceloDbContext _context;

    public LoginAttemptRepository(UceloDbContext context)
    {
        _context = context;
    }

    public async Task<LoginAttempt?> GetByEmailAsync(string email)
    {
        return await _context.LoginAttempts
            .FirstOrDefaultAsync(l => l.Email == email.ToLowerInvariant());
    }

    public async Task AddAsync(LoginAttempt attempt)
    {
        await _context.LoginAttempts.AddAsync(attempt);
    }

    public async Task UpdateAsync(LoginAttempt attempt)
    {
        _context.LoginAttempts.Update(attempt);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(LoginAttempt attempt)
    {
        _context.LoginAttempts.Remove(attempt);
        await Task.CompletedTask;
    }
}