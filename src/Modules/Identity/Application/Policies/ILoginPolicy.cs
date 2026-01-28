using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Application.Policies
{
    public interface ILoginPolicy
    {
        Task EvaluateAsync(User user, IClock clock, CancellationToken cancellationToken = default);
    }
}