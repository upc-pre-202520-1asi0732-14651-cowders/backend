using Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;

namespace Moobile_Platform.VoiceCommand.Domain.Services;

public interface IVoiceParserService
{
    VoiceCommandResult ParseCommand(string text);
}