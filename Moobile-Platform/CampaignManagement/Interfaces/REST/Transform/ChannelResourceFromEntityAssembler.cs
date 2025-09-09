using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;
using Moobile_Platform.CampaignManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.CampaignManagement.Interfaces.REST.Transform;

public static class ChannelResourceFromEntityAssembler
{
    public static ChannelResource ToResourceFromEntity(Channel channel)=>
    new ChannelResource(channel.Id, channel.Type, channel.Details, channel.CampaignId);
}