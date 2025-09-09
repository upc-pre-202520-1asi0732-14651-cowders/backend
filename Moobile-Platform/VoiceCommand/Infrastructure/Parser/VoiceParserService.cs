using System.Text.RegularExpressions;
using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;
using Moobile_Platform.VoiceCommand.Domain.Services;

namespace Moobile_Platform.VoiceCommand.Infrastructure.Parser
{
    public partial class VoiceParserService : IVoiceParserService
    {
        private readonly Dictionary<string, (VoiceCommandType type, string[] parameterNames)> _commandPatterns = new()
        {
            // Commands for user information (requires "Vicky" as the first speech word)
            [@"vicky,?\s+(?:dame\s+)?(?:mi\s+)?informaci[óo]n(?:\s+de\s+usuario)?"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:mu[eé]strame|mostrar)\s+(?:mi\s+)?(?:informaci[óo]n|perfil|datos)(?:\s+de\s+usuario)?"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:obtener|conseguir)\s+(?:mi\s+)?(?:informaci[óo]n|perfil|datos)(?:\s+de\s+usuario)?"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:quiero\s+ver|ver)\s+(?:mi\s+)?(?:informaci[óo]n|perfil|datos)(?:\s+de\s+usuario)?"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:c[óo]mo\s+est[áa]\s+)?(?:mi\s+)?(?:cuenta|perfil|usuario)"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:dime|decime)\s+(?:mi\s+)?(?:informaci[óo]n|datos)(?:\s+de\s+usuario)?"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:cu[áa]l\s+es\s+)?(?:mi\s+)?(?:estado|situaci[óo]n)(?:\s+actual)?"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:consultar|revisar)\s+(?:mi\s+)?(?:informaci[óo]n|perfil|datos)"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:resumen|sumario)\s+(?:de\s+)?(?:mi\s+)?(?:cuenta|actividad)"] = 
                (VoiceCommandType.GetUserInfo, []),
            [@"vicky,?\s+(?:estad[íi]sticas|n[úu]meros)\s+(?:de\s+)?(?:mi\s+)?(?:rancho|finca|actividad)"] = 
                (VoiceCommandType.GetUserInfo, []),

            // Navigation commands for Settings
            [@"vicky,?\s+(?:quiero\s+(?:ver|ir\s+a)|ver|ir\s+a|navegar\s+a)\s+(?:mi\s+|mis\s+|la\s+)?(?:configuraci[óo]n|config|ajustes?|opciones|preferencias)"] = 
                (VoiceCommandType.NavigateToSettings, []),
            [@"vicky,?\s+(?:ll[eé]vame\s+a|dirigir\s+a|abrir)\s+(?:la\s+secci[óo]n\s+de\s+|mis\s+|mi\s+)?(?:configuraci[óo]n|ajustes?|opciones|preferencias)"] = 
                (VoiceCommandType.NavigateToSettings, []),
            [@"vicky,?\s+(?:acceder\s+a|entrar\s+a)\s+(?:mi\s+|mis\s+)?(?:configuraci[óo]n|ajustes?|preferencias)"] = 
                (VoiceCommandType.NavigateToSettings, []),
            [@"vicky,?\s+(?:mostrar|mu[eé]strame)\s+(?:la\s+p[áa]gina\s+de\s+|mis\s+|mi\s+)?(?:configuraci[óo]n|ajustes?)"] = 
                (VoiceCommandType.NavigateToSettings, []),

            // Navigation commands for Bovines
            [@"vicky,?\s+(?:quiero\s+(?:ver|ir\s+a)|ver|ir\s+a|navegar\s+a)\s+(?:a\s+)?(?:mis\s+|la\s+gesti[óo]n\s+de\s+)?(?:bovinos?|ganado|vacas?|animales)"] = 
                (VoiceCommandType.NavigateToBovines, []),
            [@"vicky,?\s+(?:ll[eé]vame\s+a|dirigir\s+a|abrir)\s+(?:la\s+secci[óo]n\s+de\s+|la\s+gesti[óo]n\s+de\s+|mis\s+)?(?:bovinos?|ganado|vacas?|animales)"] = 
                (VoiceCommandType.NavigateToBovines, []),
            [@"vicky,?\s+(?:acceder\s+a|entrar\s+a)\s+(?:mis\s+|la\s+gesti[óo]n\s+de\s+)?(?:bovinos?|ganado|vacas?)"] = 
                (VoiceCommandType.NavigateToBovines, []),
            [@"vicky,?\s+(?:mostrar|mu[eé]strame)\s+(?:la\s+|mis\s+)?(?:lista\s+de\s+|gesti[óo]n\s+de\s+)?(?:bovinos?|ganado|vacas?|animales)"] = 
                (VoiceCommandType.NavigateToBovines, []),
            [@"vicky,?\s+(?:gestionar|administrar)\s+(?:mis\s+)?(?:bovinos?|ganado|vacas?)"] = 
                (VoiceCommandType.NavigateToBovines, []),

            // Navigation commands for Stables
            [@"vicky,?\s+(?:quiero\s+(?:ver|ir\s+a)|ver|ir\s+a|navegar\s+a)\s+(?:mis\s+|la\s+gesti[óo]n\s+de\s+)?(?:establos?|corrales?)"] = 
                (VoiceCommandType.NavigateToStables, []),
            [@"vicky,?\s+(?:ll[eé]vame\s+a|dirigir\s+a|abrir)\s+(?:la\s+secci[óo]n\s+de\s+|la\s+gesti[óo]n\s+de\s+|mis\s+)?(?:establos?|corrales?)"] = 
                (VoiceCommandType.NavigateToStables, []),
            [@"vicky,?\s+(?:acceder\s+a|entrar\s+a)\s+(?:mis\s+|la\s+gesti[óo]n\s+de\s+)?(?:establos?|corrales?)"] = 
                (VoiceCommandType.NavigateToStables, []),
            [@"vicky,?\s+(?:mostrar|mu[eé]strame)\s+(?:la\s+|mis\s+)?(?:lista\s+de\s+|gesti[óo]n\s+de\s+)?(?:establos?|corrales?)"] = 
                (VoiceCommandType.NavigateToStables, []),
            [@"vicky,?\s+(?:gestionar|administrar)\s+(?:mis\s+)?(?:establos?|corrales?)"] = 
                (VoiceCommandType.NavigateToStables, []),
            
            // Initialization to Create Stable commands - Step 1 of the CreateStable process
            [@"vicky,?\s+(?:crear?|crea|haz|hacer)\s+(?:un\s+)?(?:nuevo\s+)?establo"] = 
                (VoiceCommandType.InitializeToCreateStable, []),
            [@"vicky,?\s+(?:crear?|crea|haz|hacer)\s+(?:un\s+)?(?:nuevo\s+)?corral"] = 
                (VoiceCommandType.InitializeToCreateStable, []),
            [@"vicky,?\s+(?:nuevo|crear?)\s+establo"] = 
                (VoiceCommandType.InitializeToCreateStable, []),
            [@"vicky,?\s+quiero\s+crear\s+(?:un\s+)?(?:nuevo\s+)?establo"] = 
                (VoiceCommandType.InitializeToCreateStable, [])
        };
        // Step 2 of the CreateStable process - requires parameters "name" and "limit" of stable
        private readonly string[] _createStableStep2Patterns =
        [
            @"^(.+?)\s+con\s+l[íi]mite\s+(\d+)$",
            @"^(.+?)\s+de\s+l[íi]mite\s+(\d+)$",
            @"^(.+?)\s+l[íi]mite\s+(\d+)$",
            @"^nombre\s+(.+?)\s+(?:de|con)\s+l[íi]mite\s+(\d+)$",
            @"^(.+?)\s+capacidad\s+(\d+)$"
        ];
        
        /// <summary>
        /// Method to parse the voice command text and identify the command type and parameters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public VoiceCommandResult ParseCommand(string text)
        {
            var normalizedText = text.ToLowerInvariant().Trim();
            
            // Remove extra punctuation marks and normalize spaces
            normalizedText = MyRegex().Replace(normalizedText, " ");
            normalizedText = MyRegex1().Replace(normalizedText, " ");
            normalizedText = normalizedText.Trim();

            // First check for CreateStable Step 2 patterns
            foreach (var pattern in _createStableStep2Patterns)
            {
                var match = Regex.Match(normalizedText, pattern, RegexOptions.IgnoreCase);
                if (!match.Success) continue;
                var name = match.Groups[1].Value.Trim();
                var limitStr = match.Groups[2].Value.Trim();

                if (!int.TryParse(limitStr, out var limit) || limit <= 0 || string.IsNullOrWhiteSpace(name)) continue;
                // Clean up the name by removing common leading words
                name = MyRegexForStableName().Replace(name, "").Trim();
                        
                return new VoiceCommandResult(
                    VoiceCommandType.CreateStable,
                    new Dictionary<string, object>
                    {
                        { "name", name },
                        { "limit", limit.ToString() }
                    },
                    text,
                    true
                );
            }

            // Then check other command patterns
            foreach (var pattern in _commandPatterns)
            {
                var match = Regex.Match(normalizedText, pattern.Key, RegexOptions.IgnoreCase);
                if (!match.Success) continue;
                
                var parameters = new Dictionary<string, object>();
                var parameterNames = pattern.Value.parameterNames;
                    
                for (var i = 0; i < parameterNames.Length && i < match.Groups.Count - 1; i++)
                {
                    parameters[parameterNames[i]] = match.Groups[i + 1].Value;
                }

                return new VoiceCommandResult(
                    pattern.Value.type,
                    parameters,
                    text,
                    true
                );
            }

            return new VoiceCommandResult(
                VoiceCommandType.Unknown,
                new Dictionary<string, object>(),
                text,
                false
            );
        }
        
        /// <summary>
        /// Regexes Section
        /// </summary>
        /// <returns></returns>
        // Regex to remove punctuation marks
        [GeneratedRegex(@"[,.\-_!?;:]")]
        private static partial Regex MyRegex();
        // Regex to normalize multiple spaces to a single space
        [GeneratedRegex(@"\s+")]
        private static partial Regex MyRegex1();
        // Regex to clean up stable name by removing common leading words
        [GeneratedRegex(@"^(?:nombre\s+|el\s+|la\s+|los\s+|las\s+|de\s+|con\s+l[íi]mite.*)", RegexOptions.IgnoreCase, "es-ES")]
        private static partial Regex MyRegexForStableName();
    }
}