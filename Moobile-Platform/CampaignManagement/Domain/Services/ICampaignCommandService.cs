using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;
using Moobile_Platform.CampaignManagement.Domain.Model.Commands;

namespace Moobile_Platform.CampaignManagement.Domain.Services;

public interface ICampaignCommandService
{
    Task<Campaign?> Handle(CreateCampaignCommand command);
    
    Task<IEnumerable<Campaign>> Handle(DeleteCampaignCommand command);

    Task<Campaign?> Handle(UpdateCampaignStatusCommand command);

    Task<Campaign?> Handle(AddGoalToCampaignCommand command);
    
    Task<Campaign?> Handle(AddChannelToCampaignCommand command);

    Task<Goal?> Handle(UpdateGoalCommand command);
}