using Moobile_Platform.CampaignManagement.Domain.Model.Commands;
using Moobile_Platform.CampaignManagement.Domain.Model.ValueObjects;
using Moobile_Platform.CampaignManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.CampaignManagement.Interfaces.REST.Transform;

public static class CreateCampaignCommandFromResourceAssembler
{
    public static CreateCampaignCommand ToCommandFromResource(CreateCampaignResource resource, int userId) =>
        new CreateCampaignCommand(
            resource.Name, 
            resource.Description, 
            resource.StartDate, 
            resource.EndDate, 
            resource.Status, 
            resource.Goals, 
            resource.Channels,
            resource.StableId,
            new CampaignUserId(userId)
            );
    
}