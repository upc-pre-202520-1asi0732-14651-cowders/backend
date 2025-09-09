namespace Moobile_Platform.RanchManagement.Interfaces.REST.Resources;

public class UpdateVaccineResource
{
    public string Name { get; set; }
    public string? VaccineType { get; set; }
    public DateTime? VaccineDate { get; set; }
    public string? VaccineImg { get; set; }
    public int BovineId { get; set; }
}