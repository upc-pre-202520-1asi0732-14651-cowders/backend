namespace Moobile_Platform.StaffAdministration.Domain.Model.ValueObjects;

public record CampaignId(int CampaignIdentifier)
{
    public CampaignId() : this(0) { }
}