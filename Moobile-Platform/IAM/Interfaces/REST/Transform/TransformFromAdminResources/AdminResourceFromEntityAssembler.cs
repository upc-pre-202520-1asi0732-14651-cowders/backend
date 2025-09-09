using Moobile_Platform.IAM.Interfaces.REST.Resources.AdminResources;

namespace Moobile_Platform.IAM.Interfaces.REST.Transform.TransformFromAdminResources;

public static class AdminResourceFromEntityAssembler
{
    public static AdminResource ToResourceFromEntity(string token, string email)
    {
        return new AdminResource(token, email);
    }
}