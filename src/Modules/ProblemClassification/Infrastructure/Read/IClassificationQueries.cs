using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetActiveClassifications;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetClassificationDetail;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetClassifications;
using VAlgo.Modules.ProblemClassification.Domain.Enums;
using VAlgo.Modules.ProblemClassification.Infrastructure.Persistence;

namespace VAlgo.Modules.ProblemClassification.Infrastructure.Read
{
    public sealed class ClassificationQueries : IClassificationQueries
    {
        private readonly ClassificationDbContext _dbContext;

        public ClassificationQueries(ClassificationDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<(IReadOnlyList<ClassificationListItemDto>, int TotalCount)> GetListAsync(
            GetClassificationsQuery filter,
            int skip,
            int take,
            CancellationToken cancellationToken = default
        )
        {
            var query = _dbContext.Classifications.AsNoTracking();

            if (filter.Type.HasValue)
                query = query.Where(x => x.Type == filter.Type);

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.Name)
                .Skip(skip)
                .Take(take)
                .Select(x => new ClassificationListItemDto
                {
                    Id = x.Id.Value,
                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    IsActive = x.IsActive
                }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<ClassificationDetailDto?> GetDetailAsync(Guid classificationId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Classifications
                .AsNoTracking()
                .Where(x => x.Id.Value == classificationId)
                .Select(x => new ClassificationDetailDto
                {
                    Id = x.Id.Value,
                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<ActiveClassificationDto>, int TotalCount)> GetActiveAsync(
            ClassificationType? type,
            int skip,
            int take,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Classifications.AsNoTracking();

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Skip(skip)
                .Take(take)
                .Select(x => new ActiveClassificationDto
                {
                    Id = x.Id.Value,
                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type
                })
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

    }
}