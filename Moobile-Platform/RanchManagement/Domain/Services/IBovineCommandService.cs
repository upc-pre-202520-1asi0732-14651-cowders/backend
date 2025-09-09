using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Commands;

namespace Moobile_Platform.RanchManagement.Domain.Services;

public interface IBovineCommandService
{
    Task<Bovine?> Handle(CreateBovineCommand command);
    
    Task<Bovine?> Handle(UpdateBovineCommand command);
    
    Task<Bovine?> Handle(DeleteBovineCommand command);
}