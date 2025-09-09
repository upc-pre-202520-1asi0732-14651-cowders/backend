using Moobile_Platform.VoiceCommand.Domain.Model.Aggregates;
using Moobile_Platform.VoiceCommand.Interfaces.REST.Resources;

namespace Moobile_Platform.VoiceCommand.Interfaces.REST.Transform;

/// <summary>
/// Assembler to transform Voice entity to VoiceResource
/// </summary>
public static class VoiceResourceFromEntityAssembler
{
    /// <summary>
    /// Transforms a Voice entity to a VoiceResource
    /// </summary>
    /// <param name="entity">The Voice entity</param>
    /// <returns>VoiceResource</returns>
    public static VoiceResource ToResourceFromEntity(Voice entity)
    {
        return new VoiceResource(
            entity.Id,
            entity.OriginalText,
            entity.CommandType,
            entity.Parameters,
            entity.IsValid,
            entity.WasExecuted,
            entity.UserId,
            entity.CreatedAt,
            entity.ExecutedAt,
            entity.ErrorMessage,
            entity.ResponseMessage
        );
    }

    /// <summary>
    /// Transforms a collection of Voice entities to VoiceResources
    /// </summary>
    /// <param name="entities">Collection of Voice entities</param>
    /// <returns>Collection of VoiceResources</returns>
    public static IEnumerable<VoiceResource> ToResourcesFromEntities(IEnumerable<Voice> entities)
    {
        return entities.Select(ToResourceFromEntity);
    }
}