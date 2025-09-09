namespace Moobile_Platform.CampaignManagement.Domain.Model.ValueObjects;

public record StableId(int StableIdentifier)
{
    public StableId() : this(0) { }
};