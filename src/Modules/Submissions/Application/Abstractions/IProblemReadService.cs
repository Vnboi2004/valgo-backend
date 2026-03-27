namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface IProblemReadToSubmissionService
    {
        Task<bool> ExistsAsync(Guid problemId);
    }
}