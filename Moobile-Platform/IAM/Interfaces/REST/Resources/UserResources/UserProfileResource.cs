namespace Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;

public record UserProfileResource(
    string? Username,
    string? Email,
    bool EmailConfirmed
);