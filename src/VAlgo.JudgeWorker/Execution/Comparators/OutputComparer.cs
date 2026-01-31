namespace VAlgo.JudgeWorker.Execution.Comparators
{
    public sealed class OutputComparer
    {
        public static bool Equals(string actual, string expected)
            => Normalize(actual) == Normalize(expected);

        private static string Normalize(string value)
        {
            return value.Replace("\r", "")
                        .TrimEnd();
        }
    }
}