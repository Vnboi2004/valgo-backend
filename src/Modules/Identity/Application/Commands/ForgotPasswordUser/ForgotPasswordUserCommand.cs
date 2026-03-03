using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.ForgotPasswordUser
{
    public sealed record ForgotPasswordUserCommand(
        string Email
    ) : ICommand<Unit>;
}