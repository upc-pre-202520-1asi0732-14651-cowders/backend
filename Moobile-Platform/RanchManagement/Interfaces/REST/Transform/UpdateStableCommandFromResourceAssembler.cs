using Moobile_Platform.RanchManagement.Domain.Model.Commands;
using Moobile_Platform.RanchManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.RanchManagement.Interfaces.REST.Transform;

public static class UpdateStableCommandFromResourceAssembler
{
    public static UpdateStableCommand ToCommandFromResource(int id, UpdateStableResource resource)
    {
        return new UpdateStableCommand
        (
            Id: id,
            Name: resource.Name,
            Limit: resource.Limit
        );
    }
}