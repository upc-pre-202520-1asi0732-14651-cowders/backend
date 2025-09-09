using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;

namespace Moobile_Platform.VoiceCommand.Interfaces.REST.Resources;

/// <summary>
/// Resource for voice command statistics
/// </summary>
/// <param name="TotalCommands">Total number of commands</param>
/// <param name="ValidCommands">Number of valid commands</param>
/// <param name="ExecutedCommands">Number of successfully executed commands</param>
/// <param name="FailedCommands">Number of failed commands</param>
/// <param name="CommandsByType">Commands grouped by type</param>
public record VoiceCommandStatsResource(
    int TotalCommands,
    int ValidCommands,
    int ExecutedCommands,
    int FailedCommands,
    Dictionary<VoiceCommandType, int> CommandsByType);