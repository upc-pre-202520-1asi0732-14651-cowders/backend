namespace Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;

public record UpdateUserResource(
    string? Username,
    string? Email
);