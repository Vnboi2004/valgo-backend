namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record ReorderHintsRequest(IReadOnlyList<Guid> HintIds);
}