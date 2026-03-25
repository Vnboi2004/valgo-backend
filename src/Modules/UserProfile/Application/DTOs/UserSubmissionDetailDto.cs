using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed record UserSubmissionDetailDto
    {
        public Guid SubmissionId { get; init; }
        public DateTimeOffset SubmittedAt { get; init; }
        public Verdict Verdict { get; init; }
        public string Language { get; init; } = string.Empty;
        public int? RuntimeMs { get; init; }
        public double? MemoryMb { get; init; }
    }
}