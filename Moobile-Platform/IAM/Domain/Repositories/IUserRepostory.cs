using Moobile_Platform.IAM.Domain.Model.Aggregates;
using Moobile_Platform.Shared.Domain.Repositories;

namespace Moobile_Platform.IAM.Domain.Repositories
{
    public interface IUserRepostory : IBaseRepository<User>
    {
        Task<User?> FindByEmailAsync(string? email);
        
        Task<User?> FindByNameAsync(string? name);
        
        Task<IEnumerable<User>> FindAllAsync();
        
        Task UpdateAsync(User user);
        
        Task DeleteAsync(User user);

    }
}