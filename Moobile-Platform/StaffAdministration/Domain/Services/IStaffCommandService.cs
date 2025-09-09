using Moobile_Platform.StaffAdministration.Domain.Model.Aggregates;
using Moobile_Platform.StaffAdministration.Domain.Model.Commands;

namespace Moobile_Platform.StaffAdministration.Domain.Services;

public interface IStaffCommandService
{
    Task<Staff?> Handle(CreateStaffCommand command);
    
    Task<Staff?> Handle(UpdateStaffCommand command);
    
    Task<Staff?> Handle(DeleteStaffCommand command);
}