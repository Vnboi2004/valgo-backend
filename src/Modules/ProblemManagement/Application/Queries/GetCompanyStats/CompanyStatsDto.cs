namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyStats
{
    public sealed class CompanyStatsDto
    {
        public Guid CompanyId { get; init; }
        public string Name { get; init; } = null!;
        public int ProblemCount { get; init; }
    }
}