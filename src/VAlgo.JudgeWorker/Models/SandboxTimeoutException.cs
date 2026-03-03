namespace VAlgo.JudgeWorker.Models
{
    public sealed class SandboxTimeoutException : Exception
    {
        public SandboxTimeoutException() : base("Sandbox execution timed out") { }
    }
}