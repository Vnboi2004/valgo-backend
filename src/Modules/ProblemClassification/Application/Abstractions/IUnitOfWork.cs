namespace VAlgo.Modules.ProblemClassification.Application
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}