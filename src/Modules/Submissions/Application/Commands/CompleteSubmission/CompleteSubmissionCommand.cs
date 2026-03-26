using MediatR;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Application.Commands.CompleteSubmission
{
    public sealed class CompleteSubmissionCommand : ICommand<Unit>
    {
        public Guid SubmissionId { get; set; }
        public Verdict Verdict { get; set; }
        public int PassedTestCases { get; set; }
        public int TotalTestCases { get; set; }
        public int MaxTimeMs { get; set; }
        public int MaxMemoryKb { get; set; }
        public List<TestCaseResultDto> TestCases { get; set; } = new();
    }
}