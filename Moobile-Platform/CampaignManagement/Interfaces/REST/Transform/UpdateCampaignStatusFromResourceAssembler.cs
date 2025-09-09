using Moobile_Platform.CampaignManagement.Domain.Model.Commands;
using Moobile_Platform.CampaignManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.CampaignManagement.Interfaces.REST.Transform;

public static class UpdateCampaignStatusFromResourceAssembler
{
    public static UpdateCampaignStatusCommand ToCommandFromResource(UpdateCampaignStatusResource resource, int campaignId) =>
        new UpdateCampaignStatusCommand(campaignId, resource.Status);
}