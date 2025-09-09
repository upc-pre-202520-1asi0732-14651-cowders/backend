namespace Moobile_Platform.IAM.Application.OutBoundServices
{
    public interface IHashingService
    {
        string GenerateHash(string password);
        bool VerifyHash(string password, string hash);
    }
}