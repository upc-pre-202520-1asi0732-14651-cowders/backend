using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.Shared.Domain.Repositories;

namespace Moobile_Platform.RanchManagement.Domain.Repositories;

public interface IVaccineRepository : IBaseRepository<Vaccine>
{
    Task<Vaccine?> FindByNameAsync(string name);
    
    Task<IEnumerable<Vaccine>> FindByBovineIdAsync(int? bovineId);
    
    Task<IEnumerable<Vaccine>> FindByUserIdAsync(RanchUserId userId);
    
    Task<IEnumerable<Vaccine>> FindAllAsync();
}