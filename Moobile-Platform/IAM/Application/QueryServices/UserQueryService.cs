using Moobile_Platform.IAM.Domain.Model.Aggregates;
using Moobile_Platform.IAM.Domain.Model.Queries.UserQueries;
using Moobile_Platform.IAM.Domain.Repositories;
using Moobile_Platform.IAM.Domain.Services;
using Moobile_Platform.IAM.Interfaces.REST.Resources.UserResources;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;
using Moobile_Platform.RanchManagement.Domain.Services;

namespace Moobile_Platform.IAM.Application.QueryServices
{
    public class UserQueryService(
        IUserRepostory userRepository,
        IBovineQueryService bovineQueryService,
        IStableQueryService stableQueryService,
        //ICampaignQueryService campaignQueryService,
        IVaccineQueryService vaccineQueryService
        ) : IUserQueryService
    {
        public async Task<User?> Handle(GetUserByIdQuery query)
        {
            return await userRepository.FindByIdAsync(query.Id);
        }
        
        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
        {
            return await userRepository.FindAllAsync();
        }
        
        public async Task<User?> Handle(GetUserByEmailQuery query)
        {
            return await userRepository.FindByEmailAsync(query.Email);
        }
        
        public async Task<User?> Handle(GetUserByNameQuery query)
        {
            return await userRepository.FindByNameAsync(query.UserName);
        }

        /// <summary>
        /// Method to get user info along with statistics such as total bovines, stables, campaigns, and vaccinations.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserInfoResource?> GetUserInfoWithStatsAsync(int userId)
        {
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
                return null;

            // Obtain statistics
            var totalBovines = (await bovineQueryService.Handle(new GetAllBovinesQuery(userId))).Count();
            var totalStables = (await stableQueryService.Handle(new GetAllStablesQuery(userId))).Count();
            //var totalCampaigns = (await campaignQueryService.Handle(new GetAllCampaignsQuery(userId))).Count();
            var totalVaccinations = (await vaccineQueryService.Handle(new GetAllVaccinesQuery(userId))).Count();

            return new UserInfoResource(user.Username, totalBovines, totalVaccinations, totalStables);
        }
        
        public async Task<string?> GetUserNameByEmail(string? email)
        {
            var user = await userRepository.FindByEmailAsync(email);
            return user?.Username;
        }
        
        public async Task<string?> GetEmailByUserName(string? userName)
        {
            var user = await userRepository.FindByNameAsync(userName);
            return user?.Email;
        }
    }
}