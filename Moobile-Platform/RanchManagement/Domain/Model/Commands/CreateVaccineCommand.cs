using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;

namespace Moobile_Platform.RanchManagement.Domain.Model.Commands;

public record CreateVaccineCommand(
    string Name,
    string? VaccineType,
    DateTime? VaccineDate,
    string? VaccineImg,
    int BovineId,
    RanchUserId? RanchUserId,
    Stream? FileData);