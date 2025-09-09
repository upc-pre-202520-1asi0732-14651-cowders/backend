using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.RanchManagement.Domain.Repositories;
using Moobile_Platform.RanchManagement.Domain.Services;

namespace Moobile_Platform.RanchManagement.Application.Internal.QueryServices;

public class VaccineQueryService(IVaccineRepository vaccineRepository,
    IHttpContextAccessor httpContextAccessor) : IVaccineQueryService
{
    /// <summary>
    /// Retrieves all Vaccines
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Vaccine>> Handle(GetAllVaccinesQuery query)
    {
        // Extract user type from JWT token
        var userTypeClaim = httpContextAccessor.HttpContext?.User.FindFirst("user_type")?.Value;
        
        // If user_type is Admin, return all vaccines
        if (string.Equals(userTypeClaim, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return await vaccineRepository.FindAllAsync();
        }
        
        // If user is not Admin, filter by userId
        return await vaccineRepository.FindByUserIdAsync(new RanchUserId(query.UserId));
    }
    
    
    /// <summary>
    /// Retrieves a Vaccine entity by its unique identifier.
    /// </summary>
    /// <param name="query"></param>
    /// <returns> The Vaccine entity with the specified ID, if found; otherwise, null. </returns>
    public async Task<Vaccine?> Handle(GetVaccinesByIdQuery query)
    {
        return await vaccineRepository.FindByIdAsync(query.Id);
    }
    
    /// <summary>
    /// Retrieves all vaccines by vaccine ID.
    /// </summary>
    /// <param name="query"></param>
    /// <returns> A collection of vaccines associated with the specified vaccine ID. </returns>
    public async Task<IEnumerable<Vaccine>> Handle(GetVaccinesByBovineIdQuery query)
    {
        return await vaccineRepository.FindByBovineIdAsync(query.BovineId);
    }
}