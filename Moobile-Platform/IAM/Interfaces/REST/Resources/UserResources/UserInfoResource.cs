namespace Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;

public record UserInfoResource(
    string? Name,
    int TotalBovines,
    int TotalVaccinations,
    int TotalStables
);