namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record ReorderExamplesRequest(IReadOnlyList<Guid> ExampleIds);
}