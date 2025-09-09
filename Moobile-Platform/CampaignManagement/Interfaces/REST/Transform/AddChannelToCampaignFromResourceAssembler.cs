using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;
using Moobile_Platform.CampaignManagement.Domain.Model.Commands;
using Moobile_Platform.CampaignManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.CampaignManagement.Interfaces.REST.Transform;

public static class AddChannelToCampaignFromResourceAssembler
{
    public static AddChannelToCampaignCommand ToCommandFromResource(AddChannelToCampaignResource resource, int campaignId) =>
        new AddChannelToCampaignCommand(campaignId, new Channel(resource.Type, resource.Details, campaignId));
}