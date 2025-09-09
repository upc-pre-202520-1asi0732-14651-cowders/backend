using Moobile_Platform.IAM.Domain.Model.Commands.AdminCommands;
using Moobile_Platform.IAM.Interfaces.REST.Resources.AdminResources;

namespace Moobile_Platform.IAM.Interfaces.REST.Transform.TransformFromAdminResources;

public static class UpdateAdminCommandFromResourceAssembler
{
    public static UpdateAdminCommand ToCommandFromResource(UpdateAdminResource resource)
    {
        return new UpdateAdminCommand(resource.Email);
    }
}