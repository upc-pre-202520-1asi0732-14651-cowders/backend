using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;

namespace Moobile_Platform.VoiceCommand.Domain.Model.Commands;

/// <summary>
/// Command to create a voice command record in the database
/// </summary>
/// <param name="OriginalText">The original spoken text</param>
/// <param name="CommandType">The type of command recognized</param>
/// <param name="Parameters">Parameters extracted from the command (as JSON string)</param>
/// <param name="IsValid">Whether the command was valid</param>
/// <param name="UserId">ID of the user who issued the command</param>
public record CreateVoiceCommand(
    string OriginalText,
    VoiceCommandType CommandType,
    string? Parameters,
    bool IsValid,
    int UserId);