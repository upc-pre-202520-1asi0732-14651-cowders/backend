using System.Diagnostics;
using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Interfaces.REST.Resources;

namespace Moobile_Platform.RanchManagement.Interfaces.REST.Transform;

public static class VaccineResourceFromEntityAssembler
{
    public static VaccineResource ToResourceFromEntity(Vaccine? entity)
    {
        Debug.Assert(entity != null, nameof(entity) + " != null");
        return new VaccineResource(entity.Id,
            entity.Name,
            entity.VaccineType,
            entity.VaccineDate,
            entity.VaccineImg,
            entity.BovineId
        );
    }
}