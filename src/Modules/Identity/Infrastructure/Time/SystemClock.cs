using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Infrastructure.Time
{
    public sealed class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}