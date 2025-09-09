namespace Moobile_Platform.StaffAdministration.Domain.Model.ValueObjects;

public record StaffUserId(int UserIdentifier)
{
    public StaffUserId() : this(0) { }
}