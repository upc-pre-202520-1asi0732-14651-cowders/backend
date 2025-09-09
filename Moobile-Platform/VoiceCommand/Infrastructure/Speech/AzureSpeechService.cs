using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Moobile_Platform.VoiceCommand.Domain.Services;

namespace Moobile_Platform.VoiceCommand.Infrastructure.Speech
{
    public class AzureSpeechService : IVoiceSpeechService
    {
        private readonly SpeechConfig _speechConfig;
        private readonly ILogger<AzureSpeechService> _logger;

        public AzureSpeechService(IConfiguration configuration, ILogger<AzureSpeechService> logger)
        {
            var subscriptionKey = configuration["AzureSpeech:SubscriptionKey"];
            var region = configuration["AzureSpeech:Region"];
            
            if (string.IsNullOrEmpty(subscriptionKey) || string.IsNullOrEmpty(region))
                throw new InvalidOperationException("Azure Speech configuration is missing");
            
            _speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
            _speechConfig.SpeechRecognitionLanguage = "es-ES";
            
            // Configuration for better performance
            _speechConfig.SetProperty(PropertyId.SpeechServiceConnection_RecoMode, "INTERACTIVE");
            _speechConfig.SetProperty(PropertyId.Speech_SegmentationSilenceTimeoutMs, "5000");
            
            _logger = logger;
        }

        public async Task<string> ConvertSpeechToTextAsync(Stream audioStream)
        {
            try
            {
                _logger.LogInformation("Starting speech recognition process");
                
                // Use temporary file
                var tempFilePath = await SaveStreamToTempFileAsync(audioStream);
                
                try
                {
                    using var audioConfig = AudioConfig.FromWavFileInput(tempFilePath);
                    using var recognizer = new SpeechRecognizer(_speechConfig, audioConfig);
                    
                    _logger.LogInformation("Performing speech recognition");
                    var result = await recognizer.RecognizeOnceAsync();
                    
                    _logger.LogInformation("Recognition result: {ResultReason}", result.Reason);
                    
                    switch (result.Reason)
                    {
                        case ResultReason.RecognizedSpeech:
                            _logger.LogInformation("Recognized text: {ResultText}", result.Text);
                            return result.Text;
                        
                        case ResultReason.NoMatch:
                            _logger.LogWarning("No speech could be recognized");
                            throw new InvalidOperationException("The audio could not be recognized. Please verify that the audio contains a clear voice.");
                        
                        case ResultReason.Canceled:
                            var cancellation = CancellationDetails.FromResult(result);
                            _logger.LogError("Recognition canceled: {CancellationReason} - {CancellationErrorDetails}", cancellation.Reason, cancellation.ErrorDetails);
                            throw new InvalidOperationException($"Recognition canceled: {cancellation.ErrorDetails}");
                        
                        default:
                            throw new InvalidOperationException("Unexpected recognition result: " + result.Reason);
                    }
                }
                finally
                {
                    // Delete temporary file
                    if (File.Exists(tempFilePath))
                        File.Delete(tempFilePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during speech recognition");
                throw new InvalidOperationException($"Error in audio processing: {ex.Message}", ex);
            }
        }

        private static async Task<string> SaveStreamToTempFileAsync(Stream audioStream)
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), $"audio_{Guid.NewGuid()}.wav");

            await using var fileStream = new FileStream(tempFilePath, FileMode.Create);
            audioStream.Position = 0; // Reset stream position
            await audioStream.CopyToAsync(fileStream);
            
            return tempFilePath;
        }
    }
}