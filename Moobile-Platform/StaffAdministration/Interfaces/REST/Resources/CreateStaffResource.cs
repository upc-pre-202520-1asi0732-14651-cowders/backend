namespace Moobile_Platform.StaffAdministration.Interfaces.REST.Resources;

public record CreateStaffResource(string Name,
    int EmployeeStatus,
    int? CampaignId);