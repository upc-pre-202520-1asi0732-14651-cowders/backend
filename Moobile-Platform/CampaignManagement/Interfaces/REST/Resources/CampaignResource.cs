using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;

namespace Moobile_Platform.CampaignManagement.Interfaces.REST.Resources;

public record CampaignResource(
    int Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    ICollection<Goal> Goals,
    ICollection<Channel> Channel,
    int? StableId);