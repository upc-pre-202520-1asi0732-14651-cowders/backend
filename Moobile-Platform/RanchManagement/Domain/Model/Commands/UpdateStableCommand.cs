namespace Moobile_Platform.RanchManagement.Domain.Model.Commands;

public record UpdateStableCommand(int Id,
    string Name,
    int Limit);