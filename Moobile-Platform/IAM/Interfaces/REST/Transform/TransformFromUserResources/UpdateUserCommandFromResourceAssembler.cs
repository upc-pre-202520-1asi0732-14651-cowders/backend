using Moobile_Platform.IAM.Domain.Model.Commands.UserCommands;
using Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;

namespace Moobile_Platform.IAM.Interfaces.REST.Transform.TransformFromUserResources;

public static class UpdateUserCommandFromResourceAssembler
{
    public static UpdateUserCommand ToCommandFromResource(UpdateUserResource resource)
    {
        return new UpdateUserCommand(resource.Username, resource.Email);
    }
}