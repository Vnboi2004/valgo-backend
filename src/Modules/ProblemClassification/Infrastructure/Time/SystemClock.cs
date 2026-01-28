using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.ProblemClassification.Infrastructure.Time
{
    public sealed class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}