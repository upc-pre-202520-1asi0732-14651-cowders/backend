using Moobile_Platform.IAM.Domain.Model.Aggregates;
using Moobile_Platform.IAM.Domain.Model.Queries.AdminQueries;
using Moobile_Platform.IAM.Domain.Repositories;
using Moobile_Platform.IAM.Domain.Services;

namespace Moobile_Platform.IAM.Application.QueryServices;

public class AdminQueryService(IAdminRepository adminRepository) : IAdminQueryService
{
    public async Task<Admin?> Handle(GetAdminByIdQuery query)
    {
        return await adminRepository.FindByIdAsync(query.Id);
    }

    public async Task<Admin?> Handle(GetAdminByEmailQuery query)
    {
        return await adminRepository.FindByEmailAsync(query.Email);
    }

    public async Task<IEnumerable<Admin>> Handle(GetAllAdminsQuery query)
    {
        return await adminRepository.FindAllAsync();
    }
}