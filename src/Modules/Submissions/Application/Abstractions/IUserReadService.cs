namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface IUserReadService
    {
        Task<bool> ExistsAsync(Guid userId);
    }
}