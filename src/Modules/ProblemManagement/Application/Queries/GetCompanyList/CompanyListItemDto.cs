namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyList
{
    public sealed class CompanyListItemDto
    {
        public Guid CompanyId { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}