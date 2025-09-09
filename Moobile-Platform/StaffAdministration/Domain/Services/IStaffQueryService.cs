using Moobile_Platform.StaffAdministration.Domain.Model.Aggregates;
using Moobile_Platform.StaffAdministration.Domain.Model.Queries;

namespace Moobile_Platform.StaffAdministration.Domain.Services;

public interface IStaffQueryService
{
    Task<IEnumerable<Staff>> Handle(GetAllStaffQuery query);
    
    Task<Staff?> Handle(GetStaffByIdQuery query);
    
    Task<IEnumerable<Staff>> Handle(GetStaffByCampaignIdQuery query);
    
    Task<IEnumerable<Staff>> Handle(GetStaffByEmployeeStatusQuery query);
    
    Task<Staff?> Handle(GetStaffByNameQuery query);
}