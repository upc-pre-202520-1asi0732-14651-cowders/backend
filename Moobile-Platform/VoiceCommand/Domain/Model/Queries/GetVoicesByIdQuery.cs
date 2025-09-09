namespace Moobile_Platform.VoiceCommand.Domain.Model.Queries;

/// <summary>
/// Query to get a voice command by its ID
/// </summary>
/// <param name="Id">Voice command ID</param>
public record GetVoicesByIdQuery(int Id);