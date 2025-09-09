using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Commands;

namespace Moobile_Platform.RanchManagement.Domain.Services;

public interface IVaccineCommandService
{
    Task<Vaccine?> Handle(CreateVaccineCommand command);
    
    Task<Vaccine?> Handle(UpdateVaccineCommand command);
    
    Task<Vaccine?> Handle(DeleteVaccineCommand command);
}