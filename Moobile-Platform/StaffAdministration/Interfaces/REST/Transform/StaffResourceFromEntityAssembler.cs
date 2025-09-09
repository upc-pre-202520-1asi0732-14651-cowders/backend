using Moobile_Platform.StaffAdministration.Domain.Model.Aggregates;
using Moobile_Platform.StaffAdministration.Interfaces.REST.Resources;

namespace Moobile_Platform.StaffAdministration.Interfaces.REST.Transform;

public static class StaffResourceFromEntityAssembler
{
    public static StaffResource ToResourceFromEntity(Staff entity)
    {
        return new StaffResource(entity.Id,
            entity.Name,
            entity.EmployeeStatus.Value,
            entity.CampaignId
        );
    }
}