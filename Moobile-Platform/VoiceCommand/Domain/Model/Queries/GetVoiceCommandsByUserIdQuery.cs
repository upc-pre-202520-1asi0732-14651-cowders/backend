namespace Moobile_Platform.VoiceCommand.Domain.Model.Queries;

/// <summary>
/// Query to get all voice commands for a specific user
/// </summary>
/// <param name="UserId">User ID</param>
public record GetVoicesByUserIdQuery(int UserId);