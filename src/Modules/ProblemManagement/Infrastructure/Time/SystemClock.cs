using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Time
{
    public sealed class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}