using VAlgo.Modules.Identity.Application.Commands.LoginUser;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.RefreshTokenUser
{
    public sealed record RefreshTokenUserCommand(string RefreshToken) : ICommand<LoginResult>;
}