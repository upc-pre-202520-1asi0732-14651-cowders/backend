using Moobile_Platform.VoiceCommand.Domain.Model.Aggregates;
using Moobile_Platform.VoiceCommand.Domain.Model.Queries;
using Moobile_Platform.VoiceCommand.Infrastructure.Persistence.EFC.Repositories;

namespace Moobile_Platform.VoiceCommand.Domain.Services;

/// <summary>
/// Service interface for querying voice commands
/// </summary>
public interface IVoiceQueryService
{
    /// <summary>
    /// Handles query to get a voice command by ID
    /// </summary>
    /// <param name="query">The query containing the voice command ID</param>
    /// <returns>The voice command if found, null otherwise</returns>
    Task<Voice?> Handle(GetVoicesByIdQuery query);

    /// <summary>
    /// Handles query to get all voice commands for a user
    /// </summary>
    /// <param name="query">The query containing the user ID</param>
    /// <returns>List of voice commands for the user</returns>
    Task<IEnumerable<Voice>> Handle(GetVoicesByUserIdQuery query);

    /// <summary>
    /// Handles query to get voice commands for a user with pagination
    /// </summary>
    /// <param name="query">The query with user ID and pagination parameters</param>
    /// <returns>Paginated list of voice commands</returns>
    Task<IEnumerable<Voice>> Handle(GetVoicesByUserIdWithPaginationQuery query);

    /// <summary>
    /// Handles query to get voice commands by user and type
    /// </summary>
    /// <param name="query">The query with user ID and command type</param>
    /// <returns>List of voice commands matching the criteria</returns>
    Task<IEnumerable<Voice>> Handle(GetVoicesByUserIdAndTypeQuery query);

    /// <summary>
    /// Handles query to get voice command statistics for a user
    /// </summary>
    /// <param name="query">The query with user ID and optional date range</param>
    /// <returns>Voice command statistics</returns>
    Task<VoiceCommandStats> Handle(GetVoicesStatsByUserIdQuery query);
}