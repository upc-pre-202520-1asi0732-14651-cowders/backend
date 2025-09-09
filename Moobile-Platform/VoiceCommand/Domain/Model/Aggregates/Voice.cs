using System.ComponentModel.DataAnnotations;
using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;

namespace Moobile_Platform.VoiceCommand.Domain.Model.Aggregates;

public class Voice
{
    /// <summary>
    /// Entity Identifier
    /// </summary>
    [Required]
    public int Id { get; private set; }

    /// <summary>
    /// The original text that was spoken by the user
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string OriginalText { get; private set; }

    /// <summary>
    /// The type of command that was recognized
    /// </summary>
    [Required]
    public VoiceCommandType CommandType { get; private set; }

    /// <summary>
    /// Parameters extracted from the voice command (stored as JSON)
    /// </summary>
    [MaxLength(2000)]
    public string? Parameters { get; private set; }

    /// <summary>
    /// Whether the command was successfully recognized and executed
    /// </summary>
    [Required]
    public bool IsValid { get; private set; }

    /// <summary>
    /// Whether the command was successfully executed
    /// </summary>
    [Required]
    public bool WasExecuted { get; private set; }

    /// <summary>
    /// The user ID who issued the voice command
    /// </summary>
    [Required]
    public int UserId { get; private set; }

    /// <summary>
    /// When the command was created/issued
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// When the command was executed (if applicable)
    /// </summary>
    public DateTime? ExecutedAt { get; private set; }

    /// <summary>
    /// Error message if command execution failed
    /// </summary>
    [MaxLength(500)]
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Response message returned to user
    /// </summary>
    [MaxLength(1000)]
    public string? ResponseMessage { get; private set; }

    /// <summary>
    /// Default constructor for EF Core
    /// </summary>
    private Voice(string originalText)
    {
        OriginalText = originalText;
    }

    /// <summary>
    /// Constructor for creating a new voice command
    /// </summary>
    public Voice(string originalText, VoiceCommandType commandType, string? parameters, 
        bool isValid, int userId)
    {
        OriginalText = originalText;
        CommandType = commandType;
        Parameters = parameters;
        IsValid = isValid;
        UserId = userId;
        WasExecuted = false;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Mark the command as successfully executed
    /// </summary>
    public void MarkAsExecuted(string? responseMessage = null)
    {
        WasExecuted = true;
        ExecutedAt = DateTime.UtcNow;
        ResponseMessage = responseMessage;
        ErrorMessage = null;
    }

    /// <summary>
    /// Mark the command as failed during execution
    /// </summary>
    public void MarkAsFailed(string errorMessage)
    {
        WasExecuted = false;
        ExecutedAt = DateTime.UtcNow;
        ErrorMessage = errorMessage;
    }
}