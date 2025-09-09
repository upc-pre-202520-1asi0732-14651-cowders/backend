namespace Moobile_Platform.IAM.Domain.Model.Commands.UserCommands
{
    public record SignInCommand(
        string? Email,
        string? UserName,
        string Password
    );
}