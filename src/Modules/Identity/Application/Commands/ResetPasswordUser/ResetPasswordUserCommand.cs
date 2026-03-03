using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.ResetPasswordUser
{
    public sealed record ResetPasswordUserCommand(string Token, string NewPassword) : ICommand<Unit>;
}