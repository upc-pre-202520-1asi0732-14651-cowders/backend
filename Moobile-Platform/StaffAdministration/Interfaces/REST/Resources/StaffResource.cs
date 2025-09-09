namespace Moobile_Platform.StaffAdministration.Interfaces.REST.Resources;

public record StaffResource(int Id,
    string Name,
    int EmployeeStatus,
    int? CampaignId);