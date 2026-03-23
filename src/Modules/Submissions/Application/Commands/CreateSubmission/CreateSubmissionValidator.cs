using FluentValidation;

namespace VAlgo.Modules.Submissions.Application.Commands.CreateSubmission
{
    public sealed class CreateSubmissionValidator : AbstractValidator<CreateSubmissionCommand>
    {
        public CreateSubmissionValidator()
        {
            RuleFor(x => x.ProblemId)
                .NotEmpty();

            RuleFor(x => x.Language)
                .NotEmpty()
                .MaximumLength(32);

            RuleFor(x => x.SourceCode)
                .NotEmpty()
                .MaximumLength(100_000);
        }
    }
}