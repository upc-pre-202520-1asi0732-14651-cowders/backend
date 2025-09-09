using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.Shared.Domain.Repositories;

namespace Moobile_Platform.RanchManagement.Domain.Repositories;

public interface IBovineRepository : IBaseRepository<Bovine>
{
    Task<Bovine?> FindByNameAsync(string name);
    Task<IEnumerable<Bovine>> FindByStableIdAsync(int? stableId);
    Task<IEnumerable<Bovine>> FindByUserIdAsync(RanchUserId userId);
    Task<int> CountBovinesByStableIdAsync(int stableId);
    Task<IEnumerable<Bovine>> FindAllAsync();
}