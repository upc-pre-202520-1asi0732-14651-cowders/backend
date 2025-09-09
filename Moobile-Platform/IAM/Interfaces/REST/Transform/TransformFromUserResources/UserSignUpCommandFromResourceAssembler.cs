using Moobile_Platform.IAM.Domain.Model.Commands.UserCommands;
using Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;

namespace Moobile_Platform.IAM.Interfaces.REST.Transform.TransformFromUserResources
{
    public static class SignUpCommandFromResourceAssembler
    {
        public static SignUpCommand ToCommandFromResource(SignUpResource resource)
        {
            return new SignUpCommand(
                resource.Username,
                resource.Password,
                resource.Email
            );
        }
    }
}