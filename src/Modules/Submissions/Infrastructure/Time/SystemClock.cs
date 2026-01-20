using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Submissions.Infrastructure.Time
{
    public sealed class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}