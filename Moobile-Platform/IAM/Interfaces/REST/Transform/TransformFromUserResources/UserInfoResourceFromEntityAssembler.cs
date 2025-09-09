using Moobile_Platform.IAM.Domain.Model.Aggregates;
using Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;

namespace Moobile_Platform.IAM.Interfaces.REST.Transform.TransformFromUserResources;

public static class UserInfoResourceFromEntityAssembler
{
    public static UserInfoResource ToResourceFromEntity(
        User user,
        int totalBovines,
        //int totalCampaigns, 
        int totalVaccinations,
        int totalStables)
    {
        return new UserInfoResource(
            user.Username,
            totalBovines,
            //totalCampaigns,
            totalVaccinations,
            totalStables
        );
    }
}