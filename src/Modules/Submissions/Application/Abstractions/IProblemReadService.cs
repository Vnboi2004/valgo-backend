namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface IProblemReadService
    {
        Task<bool> ExistsAsync(Guid problemId);
    }
}