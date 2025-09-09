using Moobile_Platform.VoiceCommand.Domain.Model.Aggregates;
using Moobile_Platform.VoiceCommand.Domain.Model.Queries;
using Moobile_Platform.VoiceCommand.Domain.Repositories;
using Moobile_Platform.VoiceCommand.Domain.Services;
using Moobile_Platform.VoiceCommand.Infrastructure.Persistence.EFC.Repositories;

namespace Moobile_Platform.VoiceCommand.Application.QueryServices;

/// <summary>
/// Service implementation for querying voice commands
/// </summary>
public class VoiceQueryService(IVoiceRepository voiceDataRepository) : IVoiceQueryService
{
    /// <summary>
    /// Handles query to get a voice command by ID
    /// </summary>
    /// <param name="query">The query containing the voice command ID</param>
    /// <returns>The voice command if found, null otherwise</returns>
    public async Task<Voice?> Handle(GetVoicesByIdQuery query)
    {
        return await voiceDataRepository.FindByIdAsync(query.Id);
    }

    /// <summary>
    /// Handles query to get all voice commands for a user
    /// </summary>
    /// <param name="query">The query containing the user ID</param>
    /// <returns>List of voice commands for the user</returns>
    public async Task<IEnumerable<Voice>> Handle(GetVoicesByUserIdQuery query)
    {
        return await voiceDataRepository.FindByUserIdAsync(query.UserId);
    }

    /// <summary>
    /// Handles query to get voice commands for a user with pagination
    /// </summary>
    /// <param name="query">The query with user ID and pagination parameters</param>
    /// <returns>Paginated list of voice commands</returns>
    public async Task<IEnumerable<Voice>> Handle(GetVoicesByUserIdWithPaginationQuery query)
    {
        return await voiceDataRepository.FindByUserIdWithPaginationAsync(query.UserId, query.Page, query.Size);
    }

    /// <summary>
    /// Handles query to get voice commands by user and type
    /// </summary>
    /// <param name="query">The query with user ID and command type</param>
    /// <returns>List of voice commands matching the criteria</returns>
    public async Task<IEnumerable<Voice>> Handle(GetVoicesByUserIdAndTypeQuery query)
    {
        return await voiceDataRepository.FindByUserIdAndTypeAsync(query.UserId, query.CommandType);
    }

    /// <summary>
    /// Handles query to get voice command statistics for a user
    /// </summary>
    /// <param name="query">The query with user ID and optional date range</param>
    /// <returns>Voice command statistics</returns>
    public async Task<VoiceCommandStats> Handle(GetVoicesStatsByUserIdQuery query)
    {
        return await voiceDataRepository.GetStatsByUserIdAsync(query.UserId, query.FromDate, query.ToDate);
    }
}