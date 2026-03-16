namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemCompanies
{
    public sealed class ProblemCompanyDto
    {
        public Guid CompanyId { get; init; }
        public string Name { get; init; } = null!;
    }
}