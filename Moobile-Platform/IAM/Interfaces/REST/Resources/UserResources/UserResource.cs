namespace Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources
{
    public record UserResource(
        string token,
        string? userName,
        string? email
        );
}