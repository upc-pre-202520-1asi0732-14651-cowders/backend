namespace Moobile_Platform.StaffAdministration.Domain.Model.Commands;

public record UpdateStaffCommand(int Id,
    string Name,
    int EmployeeStatus,
    int? CampaignId);