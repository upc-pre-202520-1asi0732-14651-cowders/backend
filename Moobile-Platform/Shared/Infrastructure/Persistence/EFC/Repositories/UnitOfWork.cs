using Moobile_Platform.Shared.Domain.Repositories;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}