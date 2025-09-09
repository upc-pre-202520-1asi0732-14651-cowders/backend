namespace Moobile_Platform.VoiceCommand.Domain.Model.Queries;

/// <summary>
/// Query to get voice commands by user and command type
/// </summary>
/// <param name="UserId">User ID</param>
/// <param name="CommandType">Command type to filter by</param>
public record GetVoicesByUserIdAndTypeQuery(int UserId, VoiceCommand.Domain.Model.ValueObjects.VoiceCommandType CommandType);