using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.RegisterUser
{
    public sealed record RegisterUserCommand(
        string Username,
        string Email,
        string Password
    ) : ICommand<Guid>;
}