using Ucelo.Domain.Entities;

namespace Ucelo.Domain.Interfaces
{
    public interface IBucketRepository
    {
        Task<Bucket> GetByIdAsync(int id);
        Task<IEnumerable<Bucket>> GetAllActiveAsync();
        Task<IEnumerable<Bucket>> GetByMaterialIdAsync(int materialId);
    }
}