using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Application.Commands.RunCode
{
    public sealed class RunCodeResultDto
    {
        public string Verdict { get; set; } = default!;
        public int MaxTimeMs { get; set; }
        public int MaxMemoryKb { get; set; }
        public List<RunCodeTestCaseDto> TestCases { get; set; } = new();
    }

    public sealed class RunCodeTestCaseDto
    {
        public int Index { get; set; }
        public bool Passed { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Expected { get; set; } = string.Empty;
        public int TimeMs { get; set; }
        public int MemoryKb { get; set; }
        public string Verdict { get; set; } = default!;
    }
}