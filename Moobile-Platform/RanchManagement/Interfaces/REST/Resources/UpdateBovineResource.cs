namespace Moobile_Platform.RanchManagement.Interfaces.REST.Resources;

public class UpdateBovineResource
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Breed { get; set; }
    public string? Location { get; set; }
    public int? StableId { get; set; }
}