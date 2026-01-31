namespace VAlgo.JudgeWorker.Execution.Comparators
{
    public interface IOutputComparator
    {
        bool Compare(string expected, string actual);
    }
}