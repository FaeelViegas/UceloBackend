using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;

namespace Ucelo.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly UceloDbContext _context;

    public CompanyRepository(UceloDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies
            .AsNoTracking()
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Company?> GetByUserIdAsync(int userId)
    {
        return await _context.Companies
            .AsNoTracking()
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Company?> GetByTaxIdAsync(string taxId)
    {
        var cleanTaxId = new string(taxId.Where(char.IsDigit).ToArray());
        
        return await _context.Companies
            .AsNoTracking()
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.TaxId == cleanTaxId);
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies
            .AsNoTracking()
            .Include(c => c.User)
            .OrderBy(c => c.LegalName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Company>> GetActiveCompaniesAsync()
    {
        return await _context.Companies
            .AsNoTracking()
            .Where(c => c.IsActive)
            .Include(c => c.User)
            .OrderBy(c => c.LegalName)
            .ToListAsync();
    }

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await Task.CompletedTask;
    }

    public async Task<bool> TaxIdExistsAsync(string taxId)
    {
        var cleanTaxId = new string(taxId.Where(char.IsDigit).ToArray());
        return await _context.Companies.AnyAsync(c => c.TaxId == cleanTaxId);
    }
}