using Ucelo.Domain.Entities;

namespace Ucelo.Domain.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(int id);
    Task<Company?> GetByUserIdAsync(int userId);
    Task<Company?> GetByTaxIdAsync(string taxId);
    Task<IEnumerable<Company>> GetAllAsync();
    Task<IEnumerable<Company>> GetActiveCompaniesAsync();
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
    Task<bool> TaxIdExistsAsync(string taxId);
}