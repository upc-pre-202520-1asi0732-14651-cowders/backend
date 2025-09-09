using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;

namespace Moobile_Platform.CampaignManagement.Domain.Model.Commands;

public record AddGoalToCampaignCommand(int CampaignId, Goal Goal);