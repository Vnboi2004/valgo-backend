using VAlgo.Modules.Submissions.Application.Commands.CompleteSubmission;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.API.Controllers.Submissions.Requests
{
    public class CompleteSubmissionRequest
    {
        public int TotalTestCases { get; set; }
        public int PassedTestCases { get; set; }
        public int MaxTimeMs { get; set; }
        public int MaxMemoryKb { get; set; }
        public Verdict Verdict { get; set; }
        public List<TestCaseResultDto> TestCases { get; set; } = new();
    }
}