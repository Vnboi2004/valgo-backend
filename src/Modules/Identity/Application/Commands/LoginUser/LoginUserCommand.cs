using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.LoginUser
{
    public sealed record LoginUserCommand(
        string Email,
        string Password
    ) : ICommand<LoginResult>;
}