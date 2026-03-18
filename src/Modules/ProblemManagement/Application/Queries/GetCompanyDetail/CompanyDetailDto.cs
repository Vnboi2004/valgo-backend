namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyDetail
{
    public sealed class CompanyDetailDto
    {
        public Guid CompanyId { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}