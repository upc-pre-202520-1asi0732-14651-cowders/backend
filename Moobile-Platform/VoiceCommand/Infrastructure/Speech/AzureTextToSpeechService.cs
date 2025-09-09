using Microsoft.CognitiveServices.Speech;
using Moobile_Platform.VoiceCommand.Domain.Services;

namespace Moobile_Platform.VoiceCommand.Infrastructure.Speech
{
    public class AzureTextToSpeechService : IVoiceTextToSpeechService
    {
        private readonly SpeechConfig _speechConfig;
        private readonly ILogger<AzureTextToSpeechService> _logger;

        public AzureTextToSpeechService(IConfiguration configuration, ILogger<AzureTextToSpeechService> logger)
        {
            var subscriptionKey = configuration["AzureSpeech:SubscriptionKey"];
            var region = configuration["AzureSpeech:Region"];
            
            if (string.IsNullOrEmpty(subscriptionKey) || string.IsNullOrEmpty(region))
                throw new InvalidOperationException("Azure Speech configuration is missing");
            
            _speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
            _speechConfig.SpeechSynthesisLanguage = "es-ES";
            _speechConfig.SpeechSynthesisVoiceName = "es-MX-CandelaNeural"; // Female voice
            
            _logger = logger;
        }

        public async Task<Stream> ConvertTextToSpeechAsync(string text)
        {
            try
            {
                _logger.LogInformation("Converting text to speech: {Text}", text);
                
                using var synthesizer = new SpeechSynthesizer(_speechConfig);
                var result = await synthesizer.SpeakTextAsync(text);

                switch (result.Reason)
                {
                    case ResultReason.SynthesizingAudioCompleted:
                        _logger.LogInformation("Text-to-speech conversion completed successfully");
                        return new MemoryStream(result.AudioData);
                    
                    case ResultReason.Canceled:
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        _logger.LogError("Text-to-speech canceled: {CancellationReason} - {CancellationErrorDetails}", 
                            cancellation.Reason, cancellation.ErrorDetails);
                        throw new InvalidOperationException($"Text-to-speech canceled: {cancellation.ErrorDetails}");
                    
                    default:
                        throw new InvalidOperationException("Unexpected synthesis result: " + result.Reason);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during text-to-speech conversion");
                throw new InvalidOperationException($"Error in text-to-speech processing: {ex.Message}", ex);
            }
        }
    }
}