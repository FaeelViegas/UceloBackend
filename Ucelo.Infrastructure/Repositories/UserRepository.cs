using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;

namespace Ucelo.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UceloDbContext _context;

    public UserRepository(UceloDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Individual)
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Individual)
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Individual)
            .Include(u => u.Company)
            .OrderBy(u => u.Email)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.IsActive)
            .Include(u => u.Individual)
            .Include(u => u.Company)
            .OrderBy(u => u.Email)
            .ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await Task.CompletedTask;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email.ToLowerInvariant());
    }
}