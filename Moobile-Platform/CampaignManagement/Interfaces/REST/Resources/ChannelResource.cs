namespace Moobile_Platform.CampaignManagement.Interfaces.REST.Resources;

public record ChannelResource(
    int Id,
    string Type,
    string Details,
    int CampaignId
    );