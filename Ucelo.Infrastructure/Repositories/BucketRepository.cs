using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;

namespace Ucelo.Infrastructure.Repositories
{
    public class BucketRepository : IBucketRepository
    {
        private readonly UceloDbContext _dbContext;

        public BucketRepository(UceloDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Bucket> GetByIdAsync(int id)
        {
            return await _dbContext.Buckets
                .Include(b => b.Material)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bucket>> GetAllActiveAsync()
        {
            return await _dbContext.Buckets
                .Include(b => b.Material)
                .Where(b => b.Ativo)
                .OrderBy(b => b.Codigo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bucket>> GetByMaterialIdAsync(int materialId)
        {
            return await _dbContext.Buckets
                .Include(b => b.Material)
                .Where(b => b.MaterialId == materialId && b.Ativo)
                .OrderBy(b => b.Codigo)
                .ToListAsync();
        }
    }
}