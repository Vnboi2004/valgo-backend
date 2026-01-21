using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class ProblemWithoutTestCasesException : DomainException
    {
        public ProblemWithoutTestCasesException(Guid problemId) : base($"Problem '{problemId}' has no test cases.") { }
    }
}