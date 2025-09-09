using Moobile_Platform.Shared.Domain.Repositories;
using Moobile_Platform.StaffAdministration.Domain.Model.Aggregates;
using Moobile_Platform.StaffAdministration.Domain.Model.ValueObjects;

namespace Moobile_Platform.StaffAdministration.Domain.Repositories;

public interface IStaffRepository : IBaseRepository<Staff>
{
    Task<Staff?> FindByNameAsync(string name);
    
    Task<IEnumerable<Staff>> FindByCampaignIdAsync(int? campaignId);
    
    Task<IEnumerable<Staff>> FindByEmployeeStatusAsync(int employeeStatus);
    
    Task<IEnumerable<Staff>> FindByUserIdAsync(StaffUserId userId);
    
    Task<IEnumerable<Staff>> FindAllAsync();
}