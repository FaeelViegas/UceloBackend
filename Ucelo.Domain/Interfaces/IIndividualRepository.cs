using Ucelo.Domain.Entities;

namespace Ucelo.Domain.Interfaces;

public interface IIndividualRepository
{
    Task<Individual?> GetByIdAsync(int id);
    Task<Individual?> GetByUserIdAsync(int userId);
    Task<Individual?> GetByTaxIdAsync(string taxId);
    Task<IEnumerable<Individual>> GetAllAsync();
    Task AddAsync(Individual individual);
    Task UpdateAsync(Individual individual);
    Task<bool> TaxIdExistsAsync(string taxId);
}