using Ucelo.Domain.Entities;
using Ucelo.Domain.Enums;

namespace Ucelo.Domain.Interfaces;

public interface ICalculationRepository
{
    Task<Calculation?> GetByIdAsync(int id);
    Task<IEnumerable<Calculation>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Calculation>> GetByUserIdAndTypeAsync(int userId, CalculationType type);
    Task AddAsync(Calculation calculation);
    Task<int> CommitAsync();
}