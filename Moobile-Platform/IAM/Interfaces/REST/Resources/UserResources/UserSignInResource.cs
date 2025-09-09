namespace Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources
{
    public record SignInResource(
        string? Email,
        string? UserName,
        string Password
    );
}