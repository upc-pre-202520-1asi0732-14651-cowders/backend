using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;
using Moobile_Platform.CampaignManagement.Domain.Model.Commands;
using Moobile_Platform.CampaignManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.CampaignManagement.Interfaces.REST.Transform;

public static class AddGoalToCampaignFromResourceAssembler
{
    public static AddGoalToCampaignCommand ToCommandFromResource(AddGoalToCampaignResource resource, int campaignId) =>
        new AddGoalToCampaignCommand(campaignId, new Goal(resource.Description, resource.Metric, resource.TargetValue, resource.CurrentValue,campaignId));
};