using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Exceptions
{
    public sealed class TestCaseNotFoundException : DomainException
    {
        public TestCaseNotFoundException(TestCaseId testCaseId) : base($"TestCase {testCaseId} not found.") { }
    }
}