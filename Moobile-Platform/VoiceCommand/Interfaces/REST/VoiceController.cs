using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moobile_Platform.VoiceCommand.Domain.Model.Commands;
using Moobile_Platform.VoiceCommand.Domain.Model.Queries;
using Moobile_Platform.VoiceCommand.Domain.Services;
using Moobile_Platform.VoiceCommand.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace Moobile_Platform.VoiceCommand.Interfaces.REST
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Tags("Voice Commands")]
    public class VoiceCommandController(
        IVoiceCommandService voiceCommandService,
        IVoiceParserService parserService,
        IVoiceQueryService voiceQueryService,
        ILogger<VoiceCommandController> logger)
        : ControllerBase
    {
        [HttpPost("process-audio")]
        [SwaggerOperation(
            Summary = "Process voice command from audio file",
            Description = "Converts audio to text, executes the corresponding command, and stores the result in database. Supported formats: WAV (preferred), WebM. Returns JSON response with optional base64 audio confirmation.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Command processed successfully", typeof(object))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid audio file")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        public async Task<IActionResult> ProcessVoiceCommand(IFormFile? audioFile)
        {
            try
            {
                logger.LogInformation("Processing voice command request");
                
                // Validate audio file existence and size
                if (audioFile == null || audioFile.Length == 0)
                {
                    logger.LogWarning("No audio file provided");
                    return BadRequest(new { success = false, message = "Audio file is required" });
                }

                logger.LogInformation("Audio file info - Name: {AudioFileFileName}, Type: {AudioFileContentType}, Size: {AudioFileLength} bytes", 
                    audioFile.FileName, audioFile.ContentType, audioFile.Length);

                // Validate wav format
                var allowedTypes = new[] { "audio/wav" };
                
                if (!allowedTypes.Contains(audioFile.ContentType?.ToLower()))
                {
                    logger.LogWarning("Invalid audio format: {AudioFileContentType}", audioFile.ContentType);
                    return BadRequest(new { 
                        success = false, 
                        message = "Invalid audio format. Supported formats: WAV (preferred), WebM" 
                    });
                }

                // Validate file size (max 10MB)
                if (audioFile.Length > 10 * 1024 * 1024)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Audio file is too large. Maximum size is 10MB" 
                    });
                }

                // Get user ID from JWT
                var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    logger.LogWarning("Invalid or missing user ID in JWT");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID" });
                }

                logger.LogInformation("Processing audio for user ID: {UserId}", userId);

                // Process voice command
                await using var audioStream = audioFile.OpenReadStream();
                var command = new ProcessVoiceCommand(audioStream, userId);
                var result = await voiceCommandService.Handle(command);

                logger.LogInformation("Voice command processed successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing voice command");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Server internal error",
                    detail = ex.Message
                });
            }
        }

        [HttpPost("parse-text")]
        [SwaggerOperation(
            Summary = "Parse text command for testing",
            Description = "Parse a text command to verify command patterns without audio processing")]
        [SwaggerResponse(StatusCodes.Status200OK, "Command parsed successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        public IActionResult ParseTextCommand([FromBody] ParseTextRequest request)
        {
            try
            {
                logger.LogInformation("Parsing text command: {Text}", request.Text);
                
                if (string.IsNullOrWhiteSpace(request.Text))
                {
                    return BadRequest(new { success = false, message = "Text is required" });
                }

                var result = parserService.ParseCommand(request.Text);
                
                return Ok(new
                {
                    success = result.IsValid,
                    commandType = result.CommandType.ToString(),
                    parameters = result.Parameters,
                    originalText = result.OriginalText,
                    isValid = result.IsValid
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error parsing text command");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Server internal error",
                    detail = ex.Message
                });
            }
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all voice commands for the current user",
            Description = "Retrieves all voice commands issued by the current user, ordered by creation date")]
        [SwaggerResponse(StatusCodes.Status200OK, "Voice commands retrieved successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        public async Task<IActionResult> GetVoiceCommands()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID" });

                var query = new GetVoicesByUserIdQuery(userId.Value);
                var voiceCommands = await voiceQueryService.Handle(query);
                var resources = VoiceResourceFromEntityAssembler.ToResourcesFromEntities(voiceCommands);

                return Ok(new
                {
                    success = true,
                    data = resources,
                    count = resources.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving voice commands");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Server internal error",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("paginated")]
        [SwaggerOperation(
            Summary = "Get voice commands with pagination",
            Description = "Retrieves voice commands for the current user with pagination support")]
        [SwaggerResponse(StatusCodes.Status200OK, "Voice commands retrieved successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        public async Task<IActionResult> GetVoiceCommandsPaginated([FromQuery] int page = 0, [FromQuery] int size = 10)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID" });

                if (page < 0 || size <= 0 || size > 100)
                    return BadRequest(new { success = false, message = "Invalid pagination parameters" });

                var query = new GetVoicesByUserIdWithPaginationQuery(userId.Value, page, size);
                var voiceCommands = await voiceQueryService.Handle(query);
                var resources = VoiceResourceFromEntityAssembler.ToResourcesFromEntities(voiceCommands);

                return Ok(new
                {
                    success = true,
                    data = resources,
                    page,
                    size,
                    count = resources.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving paginated voice commands");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Server internal error",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get voice command by ID",
            Description = "Retrieves a specific voice command by its ID (only accessible by the owner)")]
        [SwaggerResponse(StatusCodes.Status200OK, "Voice command retrieved successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Voice command not found")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied - not the owner")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        public async Task<IActionResult> GetVoiceCommandById(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID" });

                var query = new GetVoicesByIdQuery(id);
                var voiceCommand = await voiceQueryService.Handle(query);

                if (voiceCommand == null)
                    return NotFound(new { success = false, message = "Voice command not found" });

                // Check if the current user is the owner
                if (voiceCommand.UserId != userId.Value)
                    return Forbid();

                var resource = VoiceResourceFromEntityAssembler.ToResourceFromEntity(voiceCommand);

                return Ok(new
                {
                    success = true,
                    data = resource
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving voice command by ID: {Id}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Server internal error",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("statistics")]
        [SwaggerOperation(
            Summary = "Get voice command statistics",
            Description = "Retrieves statistics about voice commands for the current user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Statistics retrieved successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        public async Task<IActionResult> GetVoiceCommandStatistics([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID" });

                var query = new GetVoicesStatsByUserIdQuery(userId.Value, fromDate, toDate);
                var stats = await voiceQueryService.Handle(query);
                var resource = VoiceCommandStatsResourceFromEntityAssembler.ToResourceFromStats(stats);

                return Ok(new
                {
                    success = true,
                    data = resource
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving voice command statistics");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Server internal error",
                    detail = ex.Message
                });
            }
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public record ParseTextRequest(string Text);
}