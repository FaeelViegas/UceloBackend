using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;

namespace Ucelo.Infrastructure.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly UceloDbContext _dbContext;

        public MaterialRepository(UceloDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Material> GetByIdAsync(int id)
        {
            return await _dbContext.Materials
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Material>> GetAllActiveAsync()
        {
            return await _dbContext.Materials
                .Where(m => m.Ativo)
                .OrderBy(m => m.Nome)
                .ToListAsync();
        }
    }
}