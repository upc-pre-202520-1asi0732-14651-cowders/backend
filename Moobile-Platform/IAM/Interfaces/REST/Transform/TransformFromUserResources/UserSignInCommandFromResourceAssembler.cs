using Moobile_Platform.IAM.Domain.Model.Commands.UserCommands;
using Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;

namespace Moobile_Platform.IAM.Interfaces.REST.Transform.TransformFromUserResources
{
    public static class SignInCommandFromResourceAssembler
    {
        public static SignInCommand ToCommandFromResource(SignInResource resource)
        {
            return new SignInCommand(
                resource.Email,
                resource.UserName,
                resource.Password
            );
        }
    }
}