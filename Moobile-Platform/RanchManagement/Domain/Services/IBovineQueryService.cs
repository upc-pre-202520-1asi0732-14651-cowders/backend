using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;

namespace Moobile_Platform.RanchManagement.Domain.Services;

public interface IBovineQueryService
{
    Task<IEnumerable<Bovine>> Handle(GetAllBovinesQuery query);
    
    Task<Bovine> Handle(GetBovinesByIdQuery query);
    
    Task<IEnumerable<Bovine>> Handle(GetBovinesByStableIdQuery query);
}