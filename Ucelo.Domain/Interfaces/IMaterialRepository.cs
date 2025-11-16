using Ucelo.Domain.Entities;

namespace Ucelo.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        Task<Material> GetByIdAsync(int id);
        Task<IEnumerable<Material>> GetAllActiveAsync();
    }
}