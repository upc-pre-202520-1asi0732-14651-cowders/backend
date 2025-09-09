using Moobile_Platform.RanchManagement.Domain.Model.Commands;
using Moobile_Platform.RanchManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.RanchManagement.Interfaces.REST.Transform;

public static class UpdateBovineCommandFromResourceAssembler
{
    public static UpdateBovineCommand ToCommandFromResource(int id, UpdateBovineResource resource)
    {
        return new UpdateBovineCommand
        (
            Id: id,
            Name: resource.Name,
            Gender: resource.Gender,
            BirthDate: resource?.BirthDate,
            Breed: resource?.Breed,
            Location: resource?.Location,
            StableId: resource?.StableId
        );
    }
}