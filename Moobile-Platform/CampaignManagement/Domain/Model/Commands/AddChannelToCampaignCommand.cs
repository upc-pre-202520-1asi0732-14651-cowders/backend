using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;

namespace Moobile_Platform.CampaignManagement.Domain.Model.Commands;

public record AddChannelToCampaignCommand(int CampaignId, Channel Channel);