using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;

namespace Moobile_Platform.VoiceCommand.Interfaces.REST.Resources;

/// <summary>
/// Resource representing a voice command
/// </summary>
/// <param name="Id">Voice command ID</param>
/// <param name="OriginalText">The original spoken text</param>
/// <param name="CommandType">The type of command recognized</param>
/// <param name="Parameters">Parameters extracted from the command</param>
/// <param name="IsValid">Whether the command was valid</param>
/// <param name="WasExecuted">Whether the command was successfully executed</param>
/// <param name="UserId">The user ID who issued the command</param>
/// <param name="CreatedAt">When the command was created</param>
/// <param name="ExecutedAt">When the command was executed (if applicable)</param>
/// <param name="ErrorMessage">Error message if execution failed</param>
/// <param name="ResponseMessage">Response message returned to user</param>
public record VoiceResource(
    int Id,
    string OriginalText,
    VoiceCommandType CommandType,
    string? Parameters,
    bool IsValid,
    bool WasExecuted,
    int UserId,
    DateTime CreatedAt,
    DateTime? ExecutedAt,
    string? ErrorMessage,
    string? ResponseMessage);