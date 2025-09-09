using Moobile_Platform.Shared.Domain.Repositories;
using Moobile_Platform.VoiceCommand.Domain.Model.Aggregates;
using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;
using Moobile_Platform.VoiceCommand.Infrastructure.Persistence.EFC.Repositories;

namespace Moobile_Platform.VoiceCommand.Domain.Repositories;

public interface IVoiceRepository : IBaseRepository<Voice>
{
    /// <summary>
    /// Finds all voice commands for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of voice commands for the user</returns>
    Task<IEnumerable<Voice>> FindByUserIdAsync(int userId);

    /// <summary>
    /// Finds voice commands for a user with pagination
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="page">Page number (0-based)</param>
    /// <param name="size">Page size</param>
    /// <returns>Paginated list of voice commands</returns>
    Task<IEnumerable<Voice>> FindByUserIdWithPaginationAsync(int userId, int page, int size);

    /// <summary>
    /// Finds voice commands by user and command type
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="commandType">Command type</param>
    /// <returns>List of voice commands matching the criteria</returns>
    Task<IEnumerable<Voice>> FindByUserIdAndTypeAsync(int userId, VoiceCommandType commandType);

    /// <summary>
    /// Gets recent voice commands for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="count">Number of recent commands to retrieve</param>
    /// <returns>Recent voice commands</returns>
    Task<IEnumerable<Voice>> FindRecentByUserIdAsync(int userId, int count);

    /// <summary>
    /// Counts total voice commands for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Total count of voice commands</returns>
    Task<int> CountByUserIdAsync(int userId);

    /// <summary>
    /// Counts successful voice commands for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Count of successful voice commands</returns>
    Task<int> CountSuccessfulByUserIdAsync(int userId);

    /// <summary>
    /// Counts voice commands by type for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="commandType">Command type</param>
    /// <returns>Count of commands of the specified type</returns>
    Task<int> CountByUserIdAndTypeAsync(int userId, VoiceCommandType commandType);

    /// <summary>
    /// Gets voice command statistics for a user within a date range
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="fromDate">Start date (optional)</param>
    /// <param name="toDate">End date (optional)</param>
    /// <returns>Voice command statistics</returns>
    Task<VoiceCommandStats> GetStatsByUserIdAsync(int userId, DateTime? fromDate = null, DateTime? toDate = null);
}