using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Submissions.Domain.Exceptions
{
    public sealed class SubmissionDomainException : DomainException
    {
        public SubmissionDomainException(string message) : base(message) { }
    }
}