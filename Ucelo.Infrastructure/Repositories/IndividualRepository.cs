using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;

namespace Ucelo.Infrastructure.Repositories;

public class IndividualRepository : IIndividualRepository
{
    private readonly UceloDbContext _context;

    public IndividualRepository(UceloDbContext context)
    {
        _context = context;
    }

    public async Task<Individual?> GetByIdAsync(int id)
    {
        return await _context.Individuals
            .AsNoTracking()
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Individual?> GetByUserIdAsync(int userId)
    {
        return await _context.Individuals
            .AsNoTracking()
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.UserId == userId);
    }

    public async Task<Individual?> GetByTaxIdAsync(string taxId)
    {
        var cleanTaxId = new string(taxId.Where(char.IsDigit).ToArray());
        
        return await _context.Individuals
            .AsNoTracking()
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.TaxId == cleanTaxId);
    }

    public async Task<IEnumerable<Individual>> GetAllAsync()
    {
        return await _context.Individuals
            .AsNoTracking()
            .Include(i => i.User)
            .OrderBy(i => i.FullName)
            .ToListAsync();
    }

    public async Task AddAsync(Individual individual)
    {
        await _context.Individuals.AddAsync(individual);
    }

    public async Task UpdateAsync(Individual individual)
    {
        _context.Individuals.Update(individual);
        await Task.CompletedTask;
    }

    public async Task<bool> TaxIdExistsAsync(string taxId)
    {
        var cleanTaxId = new string(taxId.Where(char.IsDigit).ToArray());
        return await _context.Individuals.AnyAsync(i => i.TaxId == cleanTaxId);
    }
}