using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Queries;

namespace Moobile_Platform.RanchManagement.Domain.Services;

public interface IStableQueryService
{
    Task<IEnumerable<Stable>> Handle(GetAllStablesQuery query);
    
    Task<Stable?> Handle(GetStablesByIdQuery query);
    
    Task<Stable?> Handle(GetStableByNameQuery query);
}