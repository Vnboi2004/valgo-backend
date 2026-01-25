namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record ReorderTestCasesRequest
    (
        IReadOnlyList<Guid> OrderedTestCaseIds
    );
}