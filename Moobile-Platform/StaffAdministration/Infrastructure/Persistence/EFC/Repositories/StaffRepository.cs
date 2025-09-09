using Microsoft.EntityFrameworkCore;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Moobile_Platform.StaffAdministration.Domain.Model.Aggregates;
using Moobile_Platform.StaffAdministration.Domain.Model.ValueObjects;
using Moobile_Platform.StaffAdministration.Domain.Repositories;

namespace Moobile_Platform.StaffAdministration.Infrastructure.Persistence.EFC.Repositories;

public class StaffRepository(AppDbContext ctx)
    : BaseRepository<Staff>(ctx), IStaffRepository
{
    public async Task<Staff?> FindByNameAsync(string name)
    {
        return await Context.Set<Staff>().FirstOrDefaultAsync(f=>f.Name == name);
    }
    
    public async Task<IEnumerable<Staff>> FindByCampaignIdAsync(int? campaignId)
    {
        return await Context.Set<Staff>().Where(f => f.CampaignId == campaignId).ToListAsync();
    }
    
    public async Task<IEnumerable<Staff>> FindByEmployeeStatusAsync(int employeeStatus)
    {
        return await Context.Set<Staff>().Where(f => f.EmployeeStatus.Value == employeeStatus).ToListAsync();
    }
    
    public async Task<IEnumerable<Staff>> FindByUserIdAsync(StaffUserId userId)
    {
        return await Context.Set<Staff>().Where(f => f.StaffUserId == userId).ToListAsync();
    }
    
    public async Task<IEnumerable<Staff>> FindAllAsync()
    {
        return await Context.Set<Staff>().ToListAsync();
    }
}