namespace Moobile_Platform.IAM.Domain.Model.Commands.UserCommands
{
    public record SignUpCommand(
        string? Username,
        string Password,
        string? Email
    );
}