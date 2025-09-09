namespace Moobile_Platform.VoiceCommand.Domain.Model.ValueObjects;

public enum VoiceCommandType
{
    // VoiceCommandTypeIndex: 0
    GetUserInfo,
    // VoiceCommandTypeIndex: 1
    NavigateToSettings,
    // VoiceCommandTypeIndex: 2
    NavigateToBovines,
    // VoiceCommandTypeIndex: 3
    NavigateToStables,
    // VoiceCommandTypeIndex: 4
    InitializeToCreateStable, // Step 1 of the CreateStable process
    // VoiceCommandTypeIndex: 5
    CreateStable, // Step 2 of the CreateStable process
    // VoiceCommandTypeIndex: 6
    Unknown
}

