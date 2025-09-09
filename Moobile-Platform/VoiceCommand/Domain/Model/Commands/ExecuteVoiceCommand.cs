using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;

namespace Moobile_Platform.VoiceCommand.Domain.Model.Commands;

/// <summary>
/// Command to execute a parsed voice command
/// </summary>
/// <param name="CommandResult">The parsed voice command result</param>
/// <param name="UserId">ID of the user issuing the command</param>
public record ExecuteVoiceCommand(VoiceCommandResult CommandResult, int UserId);