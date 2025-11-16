using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Enums;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;

namespace Ucelo.Infrastructure.Repositories;

public class CalculationRepository : ICalculationRepository
{
    private readonly UceloDbContext _context;

    public CalculationRepository(UceloDbContext context)
    {
        _context = context;
    }

    public async Task<Calculation?> GetByIdAsync(int id)
    {
        return await _context.Calculations.FindAsync(id);
    }

    public async Task<IEnumerable<Calculation>> GetByUserIdAsync(int userId)
    {
        return await _context.Calculations
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Calculation>> GetByUserIdAndTypeAsync(int userId, CalculationType type)
    {
        return await _context.Calculations
            .Where(c => c.UserId == userId && c.CalculationType == type)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Calculation calculation)
    {
        await _context.Calculations.AddAsync(calculation);
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
}