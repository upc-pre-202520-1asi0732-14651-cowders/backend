using Microsoft.EntityFrameworkCore;
using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.RanchManagement.Domain.Repositories;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Moobile_Platform.RanchManagement.Infrastructure.Persistence.EFC.Repositories;

public class VaccineRepository(AppDbContext ctx)
    : BaseRepository<Vaccine>(ctx), IVaccineRepository
{
    public async Task<Vaccine?> FindByNameAsync(string name)
    {
        return await Context.Set<Vaccine>().FirstOrDefaultAsync(f => f.Name == name);
    }
    
    public async Task<IEnumerable<Vaccine>> FindByBovineIdAsync(int? bovineId)
    {
        return await Context.Set<Vaccine>().Where(f => f.BovineId == bovineId).ToListAsync();
    }
    
    public async Task<IEnumerable<Vaccine>> FindByUserIdAsync(RanchUserId userId)
    {
        return await Context.Set<Vaccine>().Where(f => f.RanchUserId == userId).ToListAsync();
    }
    
    public async Task<IEnumerable<Vaccine>> FindAllAsync()
    {
        return await Context.Set<Vaccine>().ToListAsync();
    }
}