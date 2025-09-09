using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.Shared.Domain.Repositories;

namespace Moobile_Platform.RanchManagement.Domain.Repositories;

public interface IStableRepository : IBaseRepository<Stable>
{
    Task<Stable?> FindByNameAsync(string name);
    Task<IEnumerable<Stable>> FindByUserIdAsync(RanchUserId userId);
    Task<IEnumerable<Stable>> FindAllAsync();
}