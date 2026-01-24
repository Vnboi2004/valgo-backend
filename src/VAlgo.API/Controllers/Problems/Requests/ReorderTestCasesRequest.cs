namespace VAlgo.API.Controllers.Problems.Requests
{
    public sealed record ReorderTestCasesRequest
    (
        IReadOnlyList<Guid> OrderedTestCaseIds
    );
}