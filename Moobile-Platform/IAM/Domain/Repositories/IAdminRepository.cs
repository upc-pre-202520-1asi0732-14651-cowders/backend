using Moobile_Platform.IAM.Domain.Model.Aggregates;
using Moobile_Platform.Shared.Domain.Repositories;

namespace Moobile_Platform.IAM.Domain.Repositories;

public interface IAdminRepository: IBaseRepository<Admin>
{
    Task<Admin?> FindByEmailAsync(string email);
    Task<IEnumerable<Admin>> FindAllAsync();
    Task UpdateAsync(Admin admin);
    Task DeleteAsync(Admin admin);
}