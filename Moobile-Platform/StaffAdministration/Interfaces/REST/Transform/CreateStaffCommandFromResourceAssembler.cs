using Moobile_Platform.StaffAdministration.Domain.Model.Commands;
using Moobile_Platform.StaffAdministration.Domain.Model.ValueObjects;
using Moobile_Platform.StaffAdministration.Interfaces.REST.Resources;

namespace Moobile_Platform.StaffAdministration.Interfaces.REST.Transform;

public static class CreateStaffCommandFromResourceAssembler
{
    public static CreateStaffCommand ToCommandFromResource(CreateStaffResource resource, int userId)
    {
        return new CreateStaffCommand(
            resource.Name,
            resource.EmployeeStatus,
            resource.CampaignId,
            new StaffUserId(userId)
        );
    }
}