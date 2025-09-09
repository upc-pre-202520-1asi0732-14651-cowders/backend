using Moobile_Platform.VoiceCommand.Domain.Model.Commands;

namespace Moobile_Platform.VoiceCommand.Domain.Services;

public interface IVoiceCommandService
{
    /// <summary>
    /// Handles the creation of a voice dataCommand record
    /// </summary>
    /// <param name="dataCommand">The dataCommand to create a voice dataCommand</param>
    /// <returns>The created voice dataCommand ID</returns>
    Task<int> Handle(CreateVoiceCommand dataCommand);

    /// <summary>
    /// Handles updating the execution status of a voice command
    /// </summary>
    /// <param name="dataExecutionCommand">The command to update voice command execution</param>
    /// <returns>Success indicator</returns>
    Task<bool> Handle(UpdateVoiceExecutionCommand dataExecutionCommand);
    
    /// <summary>
    /// Handles processing of a voice data command
    /// </summary>
    /// <param name="dataCommand"></param>
    /// <returns></returns>
    Task<object> Handle(ProcessVoiceCommand dataCommand);
    
    /// <summary>
    /// Handles execution of a voice data command
    /// </summary>
    /// <param name="dataCommand"></param>
    /// <returns></returns>
    Task<object> Handle(ExecuteVoiceCommand dataCommand);
}