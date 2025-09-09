using Moobile_Platform.IAM.Domain.Model.Aggregates;

namespace Moobile_Platform.IAM.Application.OutBoundServices
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        Task<int?> ValidateToken(string token);
        string GenerateToken(Admin admin);
    }
}