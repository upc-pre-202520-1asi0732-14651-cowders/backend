using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.RanchManagement.Interfaces.REST.Transform;

public static class StableResourceFromEntityAssembler
{
    public static StableResource ToResourceFromEntity(Stable stable)
    {
        return new StableResource(
            stable.Id,
            stable.Name,
            stable.Limit
        );
    }
}