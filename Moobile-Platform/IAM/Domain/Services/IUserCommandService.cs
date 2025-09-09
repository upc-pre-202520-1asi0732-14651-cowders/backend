using Moobile_Platform.IAM.Domain.Model.Aggregates;
using Moobile_Platform.IAM.Domain.Model.Commands;
using Moobile_Platform.IAM.Domain.Model.Commands.UserCommands;

namespace Moobile_Platform.IAM.Domain.Services
{
    public interface IUserCommandService
    {
        Task<string> Handle(SignUpCommand command);
        Task<string> Handle(SignInCommand command);
        Task UpdateUserAsync(User user);
        Task<bool> Handle(UpdateUserCommand command, int userId);
        Task<bool> Handle(DeleteUserCommand command);
    }
}