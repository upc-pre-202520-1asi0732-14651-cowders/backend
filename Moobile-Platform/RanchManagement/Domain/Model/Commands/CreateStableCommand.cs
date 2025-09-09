using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;

namespace Moobile_Platform.RanchManagement.Domain.Model.Commands;

public record CreateStableCommand(
    string Name,
    int Limit,
    RanchUserId? RanchUserId);