namespace Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;

public record RanchUserId(int UserIdentifier)
{
    public RanchUserId() : this(0) { }
}