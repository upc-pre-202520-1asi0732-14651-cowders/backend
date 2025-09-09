using Moobile_Platform.StaffAdministration.Domain.Model.ValueObjects;

namespace Moobile_Platform.StaffAdministration.Domain.Model.Commands;

public record CreateStaffCommand(string Name,
    int EmployeeStatus,
    int? CampaignId,
    StaffUserId? StaffUserId);