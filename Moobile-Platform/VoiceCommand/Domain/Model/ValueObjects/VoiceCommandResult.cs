namespace Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;

public record VoiceCommandResult(VoiceCommandType CommandType,
    Dictionary<string, object> Parameters,
    string OriginalText,
    bool IsValid);