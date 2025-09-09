using Moobile_Platform.RanchManagement.Domain.Model.Commands;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.RanchManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.RanchManagement.Interfaces.REST.Transform;

public static class CreateVaccineCommandFromResourceAssembler
{
    public static CreateVaccineCommand ToCommandFromResource(CreateVaccineResource resource, int userId)
    {
        return new CreateVaccineCommand(
            resource.Name,
            resource.VaccineType,
            resource.VaccineDate,
            string.Empty,
            resource.BovineId,
            new RanchUserId(userId),
            resource.FileData?.OpenReadStream() ?? null
        );
    }
}