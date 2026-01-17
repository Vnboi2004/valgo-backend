namespace VAlgo.SharedKernel.Time;

public interface IClock
{
    DateTime UtcNow { get; }
}