using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.VerifyEmailUser
{
    public sealed record VerifyEmailUserCommand(
        string Token
    ) : ICommand<Unit>;
}