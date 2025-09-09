using Microsoft.EntityFrameworkCore;
using Moobile_Platform.IAM.Domain.Model.Aggregates;
using Moobile_Platform.IAM.Domain.Repositories;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Moobile_Platform.IAM.Infrastructure.Repositories;

public class AdminRepository(AppDbContext context) : BaseRepository<Admin>(context), IAdminRepository
{
    public async Task<Admin?> FindByEmailAsync(string email)
    {
        return await context.Set<Admin>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Admin>> FindAllAsync()
    {
        return await context.Set<Admin>().ToListAsync();
    }

    public async Task UpdateAsync(Admin admin)
    {
        context.Set<Admin>().Update(admin);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Admin admin)
    {
        context.Set<Admin>().Remove(admin);
        await context.SaveChangesAsync();
    }
}