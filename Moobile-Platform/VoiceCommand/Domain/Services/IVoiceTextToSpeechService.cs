namespace Moobile_Platform.VoiceCommand.Domain.Services;

public interface IVoiceTextToSpeechService
{
    Task<Stream> ConvertTextToSpeechAsync(string text);
}