using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;

namespace Ucelo.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly UceloDbContext _context;

    public UnitOfWork(UceloDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case Microsoft.EntityFrameworkCore.EntityState.Added:
                    entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    break;
                case Microsoft.EntityFrameworkCore.EntityState.Modified:
                case Microsoft.EntityFrameworkCore.EntityState.Deleted:
                    await entry.ReloadAsync();
                    break;
            }
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}