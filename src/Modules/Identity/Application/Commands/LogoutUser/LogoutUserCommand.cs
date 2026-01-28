using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.LogoutUser
{
    public sealed record LogoutUserCommand(string RefreshToken) : ICommand<Unit>;
}