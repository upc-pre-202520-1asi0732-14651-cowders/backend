namespace Moobile_Platform.VoiceCommand.Domain.Model.Commands;

/// <summary>
/// Command to update the execution status of a voice command
/// </summary>
/// <param name="Id">Voice command ID</param>
/// <param name="WasExecuted">Whether execution was successful</param>
/// <param name="ResponseMessage">Response message (if successful)</param>
/// <param name="ErrorMessage">Error message (if failed)</param>
public record UpdateVoiceExecutionCommand(
    int Id,
    bool WasExecuted,
    string? ResponseMessage = null,
    string? ErrorMessage = null);