namespace Moobile_Platform.VoiceCommand.Domain.Model.Queries;

/// <summary>
/// Query to get voice commands by user with pagination
/// </summary>
/// <param name="UserId">User ID</param>
/// <param name="Page">Page number (0-based)</param>
/// <param name="Size">Page size</param>
public record GetVoicesByUserIdWithPaginationQuery(int UserId, int Page = 0, int Size = 10);