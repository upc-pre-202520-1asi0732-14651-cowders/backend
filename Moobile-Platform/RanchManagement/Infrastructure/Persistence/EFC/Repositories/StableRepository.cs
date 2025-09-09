using Microsoft.EntityFrameworkCore;
using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.RanchManagement.Domain.Repositories;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Moobile_Platform.RanchManagement.Infrastructure.Persistence.EFC.Repositories;

public class StableRepository(AppDbContext ctx)
    : BaseRepository<Stable>(ctx), IStableRepository
{
    public async Task<Stable?> FindByNameAsync(string name)
    {
        return await Context.Set<Stable>().FirstOrDefaultAsync(f => f.Name == name);
    }
    
    public async Task<IEnumerable<Stable>> FindByUserIdAsync(RanchUserId userId)
    {
        return await Context.Set<Stable>().Where(f => f.RanchUserId == userId).ToListAsync();
    }
    
    public async Task<IEnumerable<Stable>> FindAllAsync()
    {
        return await Context.Set<Stable>().ToListAsync();
    }
}