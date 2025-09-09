using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;

namespace Moobile_Platform.RanchManagement.Domain.Services;

public interface IVaccineQueryService
{
    Task<IEnumerable<Vaccine>> Handle(GetAllVaccinesQuery query);
    
    Task<Vaccine?> Handle(GetVaccinesByIdQuery query);
    
    Task<IEnumerable<Vaccine>> Handle(GetVaccinesByBovineIdQuery query);
}