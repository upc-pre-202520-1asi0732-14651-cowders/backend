using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;
using Moobile_Platform.CampaignManagement.Domain.Model.ValueObjects;
using Moobile_Platform.Shared.Domain.Repositories;

namespace Moobile_Platform.CampaignManagement.Domain.Repositories;

public interface ICampaignRepository : IBaseRepository<Campaign>
{
    Task<Campaign?> FindByNameAsync(string name);
    Task<IEnumerable<Goal>> FindByCampaignId(int campaignId);
    Task<IEnumerable<Channel>> FindChannelsByCampaignId(int campaignId);
    Task<IEnumerable<Campaign>> FindByUserIdAsync(CampaignUserId userId);
    Task<IEnumerable<Campaign>> FindAllAsync();
}