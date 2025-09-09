using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Moobile_Platform.IAM.Domain.Services;
using Moobile_Platform.RanchManagement.Domain.Model.Commands;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.RanchManagement.Domain.Services;
using Moobile_Platform.Shared.Domain.Repositories;
using Moobile_Platform.VoiceCommand.Domain.Model.Aggregates;
using Moobile_Platform.VoiceCommand.Domain.Model.Commands;
using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;
using Moobile_Platform.VoiceCommand.Domain.Repositories;
using Moobile_Platform.VoiceCommand.Domain.Services;

namespace Moobile_Platform.VoiceCommand.Application.CommandServices
{
    public partial class VoiceCommandService(
        IVoiceSpeechService speechService,
        IVoiceParserService parserService,
        IVoiceTextToSpeechService textToSpeechService,
        IUserQueryService userQueryService,
        IStableQueryService stableQueryService,
        IStableCommandService stableCommandService,
        IVoiceRepository voiceDataRepository, 
        IUnitOfWork unitOfWork)
        : IVoiceCommandService
    {
        
        /// <summary>
        /// Handles the creation of a voice dataCommand record
        /// </summary>
        /// <param name="dataCommand">The dataCommand to create a voice dataCommand</param>
        /// <returns>The created voice dataCommand ID</returns>
        public async Task<int> Handle(CreateVoiceCommand dataCommand)
        {
            var voiceData = new Voice(
                dataCommand.OriginalText,
                dataCommand.CommandType,
                dataCommand.Parameters,
                dataCommand.IsValid,
                dataCommand.UserId
            );

            try
            {
                await voiceDataRepository.AddAsync(voiceData);
                await unitOfWork.CompleteAsync();
                return voiceData.Id;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed to create voice dataCommand record");
            }
        }

        /// <summary>
        /// Handles updating the execution status of a voice command
        /// </summary>
        /// <param name="dataExecutionCommand">The command to update voice command execution</param>
        /// <returns>Success indicator</returns>
        public async Task<bool> Handle(UpdateVoiceExecutionCommand dataExecutionCommand)
        {
            var voice = await voiceDataRepository.FindByIdAsync(dataExecutionCommand.Id);
            if (voice == null)
                return false;

            try
            {
                if (dataExecutionCommand.WasExecuted)
                    voice.MarkAsExecuted(dataExecutionCommand.ResponseMessage);
                else
                    voice.MarkAsFailed(dataExecutionCommand.ErrorMessage ?? "Execution failed");

                voiceDataRepository.Update(voice);
                await unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed to update voice command execution status");
            }
        }
        
        /// <summary>
        /// Valid command patterns without "Vicky" to detect if only the trigger is missing
        /// </summary>
        private readonly string[] _commandPatternsWithoutVicky =
        [
            // User info patterns without Vicky at the beginning that must be detected as errors if the word "Vicky" is not present
            @"(?:dame\s+)?(?:mi\s+)?informaci[ÃƒÆ'Ã‚Â³o]n(?:\s+de\s+usuario)?",
            @"(?:mu[eÃƒÆ'Ã‚Â©]strame|mostrar)\s+(?:mi\s+)?(?:informaci[ÃƒÆ'Ã‚Â³o]n|perfil|datos)(?:\s+de\s+usuario)?",
            @"(?:obtener|conseguir)\s+(?:mi\s+)?(?:informaci[ÃƒÆ'Ã‚Â³o]n|perfil|datos)(?:\s+de\s+usuario)?",
            @"(?:quiero\s+ver|ver)\s+(?:mi\s+)?(?:informaci[ÃƒÆ'Ã‚Â³o]n|perfil|datos)(?:\s+de\s+usuario)?",
            @"(?:c[ÃƒÆ'Ã‚Â³o]mo\s+est[ÃƒÆ'Ã‚Â¡a]\s+)?(?:mi\s+)?(?:cuenta|perfil|usuario)",
            @"(?:dime|decime)\s+(?:mi\s+)?(?:informaci[ÃƒÆ'Ã‚Â³o]n|datos)(?:\s+de\s+usuario)?",
            @"(?:cu[ÃƒÆ'Ã‚Â¡a]l\s+es\s+)?(?:mi\s+)?(?:estado|situaci[ÃƒÆ'Ã‚Â³o]n)(?:\s+actual)?",
            @"(?:consultar|revisar)\s+(?:mi\s+)?(?:informaci[ÃƒÆ'Ã‚Â³o]n|perfil|datos)",
            @"(?:resumen|sumario)\s+(?:de\s+)?(?:mi\s+)?(?:cuenta|actividad)",
            @"(?:estad[ÃƒÆ'Ã‚Â­i]sticas|n[ÃƒÆ'Ã‚Âºu]meros)\s+(?:de\s+)?(?:mi\s+)?(?:rancho|finca|actividad)",
            // Navigation patterns without Vicky at the beginning that must be detected as errors if the word "Vicky" is not present
            @"(?:quiero\s+(?:ver|ir\s+a)|ver|ir\s+a|navegar\s+a)\s+(?:mi\s+|mis\s+|la\s+)?(?:configuraci[ÃƒÆ'Ã‚Â³o]n|config|ajustes?|opciones|preferencias)",
            @"(?:ll[ÃƒÆ'Ã‚Â©e]vame\s+a|dirigir\s+a|abrir)\s+(?:la\s+secci[ÃƒÆ'Ã‚Â³o]n\s+de\s+|mis\s+|mi\s+)?(?:configuraci[ÃƒÆ'Ã‚Â³o]n|ajustes?|opciones|preferencias)",
            @"(?:acceder\s+a|entrar\s+a)\s+(?:mi\s+|mis\s+)?(?:configuraci[ÃƒÆ'Ã‚Â³o]n|ajustes?|preferencias)",
            @"(?:mostrar|mu[eÃƒÆ'Ã‚Â©]strame)\s+(?:la\s+p[ÃƒÆ'Ã‚Â¡a]gina\s+de\s+|mis\s+|mi\s+)?(?:configuraci[ÃƒÆ'Ã‚Â³o]n|ajustes?)",
            @"(?:quiero\s+(?:ver|ir\s+a)|ver|ir\s+a|navegar\s+a)\s+(?:a\s+)?(?:mis\s+|la\s+gesti[ÃƒÆ'Ã‚Â³o]n\s+de\s+)?(?:bovinos?|ganado|vacas?|animales)",
            @"(?:ll[ÃƒÆ'Ã‚Â©e]vame\s+a|dirigir\s+a|abrir)\s+(?:la\s+secci[ÃƒÆ'Ã‚Â³o]n\s+de\s+|la\s+gesti[ÃƒÆ'Ã‚Â³o]n\s+de\s+|mis\s+)?(?:bovinos?|ganado|vacas?|animales)",
            @"(?:acceder\s+a|entrar\s+a)\s+(?:mis\s+|la\s+gesti[ÃƒÆ'Ã‚Â³o]n\s+de\s+)?(?:bovinos?|ganado|vacas?)",
            @"(?:mostrar|mu[eÃƒÆ'Ã‚Â©]strame)\s+(?:la\s+|mis\s+)?(?:lista\s+de\s+|gesti[ÃƒÆ'Ã‚Â³o]n\s+de\s+)?(?:bovinos?|ganado|vacas?|animales)",
            @"(?:gestionar|administrar)\s+(?:mis\s+)?(?:bovinos?|ganado|vacas?)",
            @"(?:quiero\s+(?:ver|ir\s+a)|ver|ir\s+a|navegar\s+a)\s+(?:mis\s+|la\s+gesti[ÃƒÆ'Ã‚Â³o]n\s+de\s+)?(?:establos?|corrales?)",
            @"(?:ll[ÃƒÆ'Ã‚Â©e]vame\s+a|dirigir\s+a|abrir)\s+(?:la\s+secci[ÃƒÆ'Ã‚Â³o]n\s+de\s+|la\s+gesti[ÃƒÆ'Ã‚Â³o]n\s+de\s+|mis\s+)?(?:establos?|corrales?)",
            @"(?:acceder\s+a|entrar\s+a)\s+(?:mis\s+|la\s+gesti[ÃƒÆ'Ã‚Â³o]n\s+de\s+)?(?:establos?|corrales?)",
            @"(?:mostrar|mu[eÃƒÆ'Ã‚Â©]strame)\s+(?:la\s+|mis\s+)?(?:lista\s+de\s+|gesti[ÃƒÆ'Ã‚Â³o]n\s+de\s+)?(?:establos?|corrales?)",
            @"(?:gestionar|administrar)\s+(?:mis\s+)?(?:establos?|corrales?)",
            // Patterns for creating a stable without Vicky at the beginning that must be detected as errors if the word "Vicky" is not present
            @"(?:crear?|crea|haz|hacer)\s+(?:un\s+)?(?:nuevo\s+)?establo",
            @"(?:nuevo|crear?)\s+establo",
            @"quiero\s+crear\s+(?:un\s+)?(?:nuevo\s+)?establo"
        ];
        
        public async Task<object> Handle(ProcessVoiceCommand command)
        {
            int? voiceCommandId = null;

            try
            {
                // Step 1: Convert speech to text
                var text = await speechService.ConvertSpeechToTextAsync(command.AudioStream);

                // Step 2: Parse the text to a command
                var commandResult = parserService.ParseCommand(text);

                // Step 3: Create voice command record in database
                var createVoiceCommand = new CreateVoiceCommand(
                    text,
                    commandResult.CommandType,
                    commandResult.Parameters.Count != 0 ? JsonSerializer.Serialize(commandResult.Parameters) : null,
                    commandResult.IsValid,
                    command.UserId
                );

                voiceCommandId = await this.Handle(createVoiceCommand);

                // Step 4: Check if command is valid
                if (!commandResult.IsValid)
                {
                    var errorMessage = IsCommandMissingVicky(text)
                        ? "The order has to go after calling Vicky first"
                        : "Command not recognized";

                    await this.Handle(new UpdateVoiceExecutionCommand(
                        voiceCommandId.Value, false, null, errorMessage));

                    return new
                    {
                        success = false,
                        message = errorMessage,
                        recognizedText = text,
                        suggestion = IsCommandMissingVicky(text) ? $"Try saying: 'Vicky, {text.Trim()}'" : null,
                        voiceCommandId
                    };
                }

                // Step 5: Execute the command first to get the actual message
                var executeCommand = new ExecuteVoiceCommand(commandResult, command.UserId);
                var result = await Handle(executeCommand);

                // Step 6: Determine which message to use for audio
                var wasSuccessful = HasSuccessProperty(result) && GetSuccessValue(result);
                var responseMessage = GetMessageValue(result);
                var requiresFollowUp = GetRequiresFollowUpValue(result);
                
                // For commands that require follow-up, use the actual response message for audio
                // For other commands, use the confirmation message
                var audioMessage = requiresFollowUp ? responseMessage : GetConfirmationMessage(commandResult.CommandType);
                
                // Step 7: Generate audio with the appropriate message
                try
                {
                    Debug.Assert(audioMessage != null, nameof(audioMessage) + " != null");
                    var audioStream = await textToSpeechService.ConvertTextToSpeechAsync(audioMessage);
                    using var memoryStream = new MemoryStream();
                    await audioStream.CopyToAsync(memoryStream);
                }
                catch
                {
                    // Log error but don't fail the entire operation
                    // logger.LogWarning("Failed to generate audio");
                }

                // Step 8: Update voice command execution status
                var message = wasSuccessful ? null : responseMessage;

                await this.Handle(new UpdateVoiceExecutionCommand(
                    voiceCommandId.Value, wasSuccessful, wasSuccessful ? responseMessage : null, message));

                // Step 9: Generate follow-up success message after a delay (only for successful operations excluding CreateStable and InitializeToCreateStable)
                if (wasSuccessful && 
                    commandResult.CommandType != VoiceCommandType.CreateStable && 
                    commandResult.CommandType != VoiceCommandType.InitializeToCreateStable)
                {
                    // Schedule follow-up message after 0.5 seconds
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await Task.Delay(500);
                            const string followUpMessage = "La operación se realizó correctamente, estaré encantada de seguir atendiendo tus solicitudes";
                            var followUpAudioStream = await textToSpeechService.ConvertTextToSpeechAsync(followUpMessage);
                            using var followUpMemoryStream = new MemoryStream();
                            await followUpAudioStream.CopyToAsync(followUpMemoryStream);
                        }
                        catch
                        {
                            // Log error but don't affect the main response
                            // logger.LogWarning("Failed to generate follow-up audio");
                        }
                    });
                }

                // Step 10: Prepare response
                Debug.Assert(responseMessage != null, nameof(responseMessage) + " != null");
                var response = new Dictionary<string, object>
                {
                    ["success"] = wasSuccessful,
                    ["message"] = responseMessage,
                    ["originalText"] = commandResult.OriginalText,
                    ["voiceCommandId"] = voiceCommandId
                };

                // Add other response data if available
                var dataValue = GetDataValue(result);
                if (dataValue != null)
                {
                    response["data"] = dataValue;
                }

                var redirectUrlValue = GetRedirectUrlValue(result);
                if (!string.IsNullOrEmpty(redirectUrlValue))
                {
                    response["redirectUrl"] = redirectUrlValue;
                }

                return response;
            }
            catch (Exception ex)
            {
                // Update voice command as failed if we have an ID
                if (!voiceCommandId.HasValue)
                    return new
                    {
                        success = false,
                        message = ex.Message,
                        voiceCommandId
                    };
                try
                {
                    await Handle(new UpdateVoiceExecutionCommand(
                        voiceCommandId.Value, false, null, ex.Message));
                }
                catch
                {
                    // Ignore errors when updating failed status
                }

                return new
                {
                    success = false,
                    message = ex.Message,
                    voiceCommandId
                };
            }
        }
        
        public async Task<object> Handle(ExecuteVoiceCommand command) 
        {
            try
            {
                var result = command.CommandResult;

                var response = result.CommandType switch
                {
                    VoiceCommandType.GetUserInfo => await ExecuteGetUserInfo(command.UserId),
                    VoiceCommandType.NavigateToSettings => ExecuteNavigateToSettings(),
                    VoiceCommandType.NavigateToBovines => ExecuteNavigateToBovines(),
                    VoiceCommandType.NavigateToStables => ExecuteNavigateToStables(),
                    VoiceCommandType.InitializeToCreateStable => ExecuteCreateStableStep1(),
                    VoiceCommandType.CreateStable => await ExecuteCreateStableStep2(command.UserId, 
                        result.Parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString() ?? string.Empty)),
                    _ => new
                    {
                        success = false,
                        message = $"Command '{result.CommandType}' not implemented yet",
                        originalText = result.OriginalText
                    }
                };

                return new
                {
                    success = GetSuccessValue(response),
                    message = GetMessageValue(response),
                    data = GetDataValue(response),
                    redirectUrl = GetRedirectUrlValue(response),
                    originalText = result.OriginalText,
                    requiresFollowUp = GetRequiresFollowUpValue(response)
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        private static object ExecuteCreateStableStep1()
        {
            return new
            {
                success = true,
                message = "Ok, indícame el nombre y el límite del establo que deseas crear",
                requiresFollowUp = true
            };
        }

        private async Task<object> ExecuteCreateStableStep2(int userId, Dictionary<string, string> parameters)
        {
            try
            {
                if (!parameters.TryGetValue("name", out var stableName) || 
                    !parameters.TryGetValue("limit", out var limitStr))
                {
                    return new
                    {
                        success = false,
                        message = "No pude entender el nombre o límite del establo. Intenta nuevamente con el formato: 'nombre del establo con límite número'"
                    };
                }

                if (!int.TryParse(limitStr, out var limit) || limit <= 0)
                {
                    return new
                    {
                        success = false,
                        message = "El límite debe ser un número mayor a cero"
                    };
                }

                // Verifies if the stable name already exists
                try
                {
                    var existingStable = await stableQueryService.Handle(new GetStableByNameQuery(stableName));
                    if (existingStable != null)
                    {
                        return new
                        {
                            success = false,
                            message = $"Ya existe un establo con el nombre '{stableName}'. Elige un nombre diferente"
                        };
                    }
                }
                catch
                {
                    // If the query fails, we assume the name is available
                }

                // Create the stable using the command service
                var createStableCommand = new CreateStableCommand(stableName, limit, new RanchUserId(userId));
                var stableResult = await stableCommandService.Handle(createStableCommand);

                if (stableResult is not { Id: > 0 })
                    return new
                    {
                        success = false,
                        message = "El establo no fue creado debido a un error inesperado"
                    };
                
                var stableId = stableResult.Id;
                return new
                {
                    success = true,
                    message = "El establo fue creado satisfactoriamente, estaré atenta y encantada de ayudarte con lo que necesites",
                    data = new { stableId },
                    redirectUrl = $"https://vac-app-frontend-web.netlify.app/stables/{stableId}"
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        /// <summary>
        /// Method to extract properties from anonymous objects
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static bool GetRequiresFollowUpValue(object response)
        {
            var type = response.GetType();
            var requiresFollowUpProperty = type.GetProperty("requiresFollowUp");
            return requiresFollowUpProperty?.GetValue(response) as bool? ?? false;
        }

        /// <summary>
        /// Method to get user info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<object> ExecuteGetUserInfo(int userId)
        {
            try
            {
                var userInfo = await userQueryService.GetUserInfoWithStatsAsync(userId);
                
                if (userInfo == null)
                {
                    return new
                    {
                        success = false,
                        message = "User not found"
                    };
                }

                return new
                {
                    success = true,
                    message = $"Perfecto, tienes un total de {userInfo.TotalBovines} bovino{(userInfo.TotalBovines != 1 ? "s" : "")}, {userInfo.TotalVaccinations} vacuna{(userInfo.TotalVaccinations != 1 ? "s" : "")} y {userInfo.TotalStables} establo{(userInfo.TotalStables != 1 ? "s" : "")}",
                    data = new
                    {
                        username = userInfo.Name,
                        totalBovines = userInfo.TotalBovines,
                        totalVaccinations = userInfo.TotalVaccinations,
                        totalStables = userInfo.TotalStables
                    }
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = $"Error retrieving user information: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Method to navigate to settings
        /// </summary>
        /// <returns></returns>
        private static object ExecuteNavigateToSettings()
        {
            return new
            {
                success = true,
                message = "De acuerdo, dirigiéndote a la configuración",
                redirectUrl = "https://vac-app-frontend-web.netlify.app/settings"
            };
        }

        /// <summary>
        /// Method to navigate to bovines management
        /// </summary>
        /// <returns></returns>
        private static object ExecuteNavigateToBovines()
        {
            return new
            {
                success = true,
                message = "Perfecto, te llevaré a la gestión de bovinos",
                redirectUrl = "https://vac-app-frontend-web.netlify.app/bovines"
            };
        }

        /// <summary>
        /// Method to navigate to stables management
        /// </summary>
        /// <returns></returns>
        private static object ExecuteNavigateToStables()
        {
            return new
            {
                success = true,
                message = "Entendido, te llevaré a la gestión de establos",
                redirectUrl = "https://vac-app-frontend-web.netlify.app/stables"
            };
        }

        /// <summary>
        /// Method to get a confirmation message based on command type
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        private static string GetConfirmationMessage(VoiceCommandType commandType)
        {
            return commandType switch
            {
                VoiceCommandType.GetUserInfo => "Solicitud aceptada, procediendo a obtener tu información de usuario",
                VoiceCommandType.NavigateToSettings => "De acuerdo, dirigiéndote a la configuración",
                VoiceCommandType.NavigateToBovines => "Perfecto, te llevaré a la gestión de bovinos",
                VoiceCommandType.NavigateToStables => "Entendido, te llevaré a la gestión de establos",
                VoiceCommandType.InitializeToCreateStable => "Solicitud aceptada, procediendo a crear un nuevo establo",
                VoiceCommandType.CreateStable => "Procesando la creación del establo",
                _ => "Solicitud aceptada, procediendo a cumplir con la orden"
            };
        }

        private static bool HasSuccessProperty(object response)
        {
            var type = response.GetType();
            return type.GetProperty("success") != null;
        }

        private static bool GetSuccessValue(object response)
        {
            var type = response.GetType();
            var successProperty = type.GetProperty("success");
            return successProperty?.GetValue(response) as bool? ?? false;
        }

        private static string? GetMessageValue(object response)
        {
            var type = response.GetType();
            var messageProperty = type.GetProperty("message");
            return messageProperty?.GetValue(response)?.ToString();
        }

        private static object? GetDataValue(object response)
        {
            var type = response.GetType();
            var dataProperty = type.GetProperty("data");
            return dataProperty?.GetValue(response);
        }

        private static string? GetRedirectUrlValue(object response)
        {
            var type = response.GetType();
            var redirectUrlProperty = type.GetProperty("redirectUrl");
            return redirectUrlProperty?.GetValue(response)?.ToString();
        }

        private bool IsCommandMissingVicky(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) 
                return false;

            var normalizedText = text.ToLowerInvariant().Trim();
            
            // If it already starts with "vicky", it's all okay
            if (normalizedText.StartsWith("vicky"))
                return false;

            // Remove punctuation marks and normalize spaces
            normalizedText = MyRegex().Replace(normalizedText, " ");
            normalizedText = MyRegex1().Replace(normalizedText, " ");
            normalizedText = normalizedText.Trim();

            // Check if the text matches a valid command pattern (without Vicky)
            return _commandPatternsWithoutVicky.Any(pattern => Regex.IsMatch(normalizedText, $"^{pattern}$", RegexOptions.IgnoreCase));
        }

        [GeneratedRegex(@"[,.\-_!?;:]")]
        private static partial Regex MyRegex();
        
        [GeneratedRegex(@"\s+")]
        private static partial Regex MyRegex1();
    }
}