using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class CannotRemoveLastSampleTestCaseException : DomainException
    {
        public CannotRemoveLastSampleTestCaseException(Guid problemId) : base($"Problem '{problemId}' must have at least one sample test case.") { }
    }
}