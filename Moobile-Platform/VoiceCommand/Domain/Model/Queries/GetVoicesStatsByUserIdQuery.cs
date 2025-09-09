namespace Moobile_Platform.VoiceCommand.Domain.Model.Queries;

/// <summary>
/// Query to get voice command statistics for a user
/// </summary>
/// <param name="UserId">User ID</param>
/// <param name="FromDate">Start date for statistics (optional)</param>
/// <param name="ToDate">End date for statistics (optional)</param>
public record GetVoicesStatsByUserIdQuery(int UserId, DateTime? FromDate = null, DateTime? ToDate = null);