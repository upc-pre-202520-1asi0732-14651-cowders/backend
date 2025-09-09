namespace Moobile_Platform.VoiceCommand.Domain.Model.Commands;

/// <summary>
/// Command to process voice input from audio stream
/// </summary>
/// <param name="AudioStream">Audio stream containing the voice command</param>
/// <param name="UserId">ID of the user issuing the command</param>
public record ProcessVoiceCommand(Stream AudioStream, int UserId);