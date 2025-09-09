using Moobile_Platform.RanchManagement.Domain.Model.Commands;
using Moobile_Platform.RanchManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.RanchManagement.Interfaces.REST.Transform;

public static class UpdateVaccineCommandFromResourceAssembler
{
    public static UpdateVaccineCommand ToCommandFromResource(int id, UpdateVaccineResource resource)
    {
        return new UpdateVaccineCommand
        (
            Id: id,
            Name: resource.Name,
            VaccineType: resource.VaccineType,
            VaccineDate: resource.VaccineDate,
            BovineId: resource.BovineId
        );
    }
}