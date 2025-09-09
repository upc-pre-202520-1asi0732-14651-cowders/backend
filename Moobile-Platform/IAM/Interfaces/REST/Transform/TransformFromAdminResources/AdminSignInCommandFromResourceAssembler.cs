using Moobile_Platform.IAM.Domain.Model.Commands.AdminCommands;
using Moobile_Platform.IAM.Interfaces.REST.Resources.AdminResources;

namespace Moobile_Platform.IAM.Interfaces.REST.Transform.TransformFromAdminResources;

public static class AdminSignInCommandFromResourceAssembler
{
    public static AdminSignInCommand ToCommandFromResource(AdminSignInResource resource)
    {
        return new AdminSignInCommand(resource.Email, resource.Password);
    }
}