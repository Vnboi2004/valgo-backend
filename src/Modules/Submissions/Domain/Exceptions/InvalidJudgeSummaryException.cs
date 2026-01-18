using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Submissions.Domain.Exceptions
{
    public sealed class InvalidJudgeSummaryException : DomainException
    {
        public InvalidJudgeSummaryException(string message) : base(message) { }
    }
}