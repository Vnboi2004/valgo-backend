using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AddAllowedLanguage
{
    public sealed record AddAllowedLanguageCommand(
        Guid ProblemId,
        string Language
    ) : ICommand<Unit>;
}