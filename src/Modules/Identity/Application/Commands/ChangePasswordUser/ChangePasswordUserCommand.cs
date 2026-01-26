using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.ChangePasswordUser
{
    public sealed record ChangePasswordUserCommand(
        Guid UserId,
        string CurrentPassword,
        string NewPassword
    ) : ICommand<Unit>;
}