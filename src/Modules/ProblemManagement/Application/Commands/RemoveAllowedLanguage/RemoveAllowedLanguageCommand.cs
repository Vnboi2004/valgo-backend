using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.RemoveAllowedLanguage
{
    public sealed record RemoveAllowedLanguageCommand(Guid ProblemId, string Language) : ICommand<Unit>;
}