using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.RanchManagement.Domain.Repositories;
using Moobile_Platform.RanchManagement.Domain.Services;

namespace Moobile_Platform.RanchManagement.Application.Internal.QueryServices;

public class BovineQueryService(
    IBovineRepository bovineRepository, 
    IHttpContextAccessor httpContextAccessor) : IBovineQueryService
{
    /// <summary>
    /// Retrieves all Bovines based on user role
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Bovine>> Handle(GetAllBovinesQuery query)
    {
        // Extract user type from JWT token
        var userTypeClaim = httpContextAccessor.HttpContext?.User.FindFirst("user_type")?.Value;
        
        // If user_type is Admin, return all bovines
        if (string.Equals(userTypeClaim, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return await bovineRepository.FindAllAsync();
        }
        
        // If user is not Admin, filter by userId
        return await bovineRepository.FindByUserIdAsync(new RanchUserId(query.UserId));
    }
    
    /// <summary>
    /// Retrieves a Bovine entity by its unique identifier.
    /// </summary>
    /// <param name="query"></param>
    /// <returns> The Bovine entity with the specified ID, if found; otherwise, null. </returns>
    public async Task<Bovine> Handle(GetBovinesByIdQuery query)
    {
        return await bovineRepository.FindByIdAsync(query.Id);
    }
    
    /// <summary>
    /// Retrieves all bovines by stable ID.
    /// </summary>
    /// <param name="query"></param>
    /// <returns> A collection of bovines associated with the specified stable ID. </returns>
    public async Task<IEnumerable<Bovine>> Handle(GetBovinesByStableIdQuery query)
    {
        return await bovineRepository.FindByStableIdAsync(query.StableId);
    }
}