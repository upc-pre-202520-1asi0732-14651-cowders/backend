using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.RanchManagement.Domain.Repositories;
using Moobile_Platform.RanchManagement.Domain.Services;

namespace Moobile_Platform.RanchManagement.Application.Internal.QueryServices;

public class StableQueryService(IStableRepository stableRepository,
    IHttpContextAccessor httpContextAccessor) : IStableQueryService
{
    /// <summary>
    /// Retrieves all Stables
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Stable>> Handle(GetAllStablesQuery query)
    {
        // Extract user type from JWT token
        var userTypeClaim = httpContextAccessor.HttpContext?.User.FindFirst("user_type")?.Value;
        
        // If user_type is Admin, return all vaccines
        if (string.Equals(userTypeClaim, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return await stableRepository.FindAllAsync();
        }
        
        // If user is not Admin, filter by userId
        return await stableRepository.FindByUserIdAsync(new RanchUserId(query.UserId));
    }
    
    /// <summary>
    /// Retrieves a Stable entity by its unique identifier.
    /// </summary>
    /// <param name="query"></param>
    /// <returns> The Stable entity with the specified ID, if found; otherwise, null. </returns>
    public async Task<Stable?> Handle(GetStablesByIdQuery query)
    {
        return await stableRepository.FindByIdAsync(query.Id);
    }

    /// <summary>
    /// Retrieves a Stable entity by its name.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Stable?> Handle(GetStableByNameQuery query)
    {
        return await stableRepository.FindByNameAsync(query.Name);
    }
}