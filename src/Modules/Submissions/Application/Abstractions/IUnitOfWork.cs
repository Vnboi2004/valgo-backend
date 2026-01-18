namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}