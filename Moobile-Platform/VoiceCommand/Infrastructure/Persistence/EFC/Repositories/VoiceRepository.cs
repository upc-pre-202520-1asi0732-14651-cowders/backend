using Microsoft.EntityFrameworkCore;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Moobile_Platform.VoiceCommand.Domain.Model.Aggregates;
using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;
using Moobile_Platform.VoiceCommand.Domain.Repositories;

namespace Moobile_Platform.VoiceCommand.Infrastructure.Persistence.EFC.Repositories;

public class VoiceRepository(AppDbContext context) 
    : BaseRepository<Voice>(context), IVoiceRepository
{
    /// <summary>
    /// Finds all voice commands for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of voice commands for the user</returns>
    public async Task<IEnumerable<Voice>> FindByUserIdAsync(int userId)
    {
        return await Context.Set<Voice>()
            .Where(vc => vc.UserId == userId)
            .OrderByDescending(vc => vc.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Finds voice commands for a user with pagination
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="page">Page number (0-based)</param>
    /// <param name="size">Page size</param>
    /// <returns>Paginated list of voice commands</returns>
    public async Task<IEnumerable<Voice>> FindByUserIdWithPaginationAsync(int userId, int page, int size)
    {
        return await Context.Set<Voice>()
            .Where(vc => vc.UserId == userId)
            .OrderByDescending(vc => vc.CreatedAt)
            .Skip(page * size)
            .Take(size)
            .ToListAsync();
    }

    /// <summary>
    /// Finds voice commands by user and command type
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="commandType">Command type</param>
    /// <returns>List of voice commands matching the criteria</returns>
    public async Task<IEnumerable<Voice>> FindByUserIdAndTypeAsync(int userId, Domain.Model.ValueObjects.VoiceCommandType commandType)
    {
        return await Context.Set<Voice>()
            .Where(vc => vc.UserId == userId && vc.CommandType == commandType)
            .OrderByDescending(vc => vc.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Gets recent voice commands for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="count">Number of recent commands to retrieve</param>
    /// <returns>Recent voice commands</returns>
    public async Task<IEnumerable<Voice>> FindRecentByUserIdAsync(int userId, int count)
    {
        return await Context.Set<Voice>()
            .Where(vc => vc.UserId == userId)
            .OrderByDescending(vc => vc.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    /// <summary>
    /// Counts total voice commands for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Total count of voice commands</returns>
    public async Task<int> CountByUserIdAsync(int userId)
    {
        return await Context.Set<Voice>()
            .CountAsync(vc => vc.UserId == userId);
    }

    /// <summary>
    /// Counts successful voice commands for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Count of successful voice commands</returns>
    public async Task<int> CountSuccessfulByUserIdAsync(int userId)
    {
        return await Context.Set<Voice>()
            .CountAsync(vc => vc.UserId == userId && vc.IsValid && vc.WasExecuted);
    }

    /// <summary>
    /// Counts voice commands by type for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="commandType">Command type</param>
    /// <returns>Count of commands of the specified type</returns>
    public async Task<int> CountByUserIdAndTypeAsync(int userId, VoiceCommandType commandType)
    {
        return await Context.Set<Voice>()
            .CountAsync(vc => vc.UserId == userId && vc.CommandType == commandType);
    }

    /// <summary>
    /// Gets voice command statistics for a user within a date range
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="fromDate">Start date (optional)</param>
    /// <param name="toDate">End date (optional)</param>
    /// <returns>Voice command statistics</returns>
    public async Task<VoiceCommandStats> GetStatsByUserIdAsync(int userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = Context.Set<Voice>()
            .Where(vc => vc.UserId == userId);

        if (fromDate.HasValue)
            query = query.Where(vc => vc.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(vc => vc.CreatedAt <= toDate.Value);

        var commands = await query.ToListAsync();

        return new VoiceCommandStats
        {
            TotalCommands = commands.Count,
            ValidCommands = commands.Count(vc => vc.IsValid),
            ExecutedCommands = commands.Count(vc => vc.WasExecuted),
            FailedCommands = commands.Count(vc => vc.IsValid && !vc.WasExecuted),
            CommandsByType = commands.GroupBy(vc => vc.CommandType)
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }
}

/// <summary>
/// Voice command statistics data transfer object
/// </summary>
public class VoiceCommandStats
{
    public int TotalCommands { get; set; }
    public int ValidCommands { get; set; }
    public int ExecutedCommands { get; set; }
    public int FailedCommands { get; set; }
    public Dictionary<VoiceCommandType, int> CommandsByType { get; set; } = new();
}