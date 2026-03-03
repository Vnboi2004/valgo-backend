using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class InvalidTestCaseException : DomainException
    {
        public InvalidTestCaseException(string reason) : base($"Invalid test case: {reason}.") { }
    }
}