namespace VAlgo.JudgeWorker.Execution.Comparators
{
    public sealed class ExactComparator : IOutputComparator
    {
        public bool Compare(string expected, string actual)
            => expected == actual;
    }
}