using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemClassification.Infrastructure.Persistence;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.SharedKernel.CrossModule.Classifications;
using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.CrossModule.Submissions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.UserProfile.Infrastructure.Services
{
    public sealed class UserProfileReadService : IUserProfileReadService
    {
        private readonly SubmissionsDbContext _submissionsDbContext;
        private readonly ProblemManagementDbContext _problemDbContext;
        private readonly ClassificationDbContext _classificationDbContext;

        public UserProfileReadService(
            SubmissionsDbContext submissionDbContext,
            ProblemManagementDbContext problemDbContext,
            ClassificationDbContext classificationDbContext)
        {
            _submissionsDbContext = submissionDbContext;
            _problemDbContext = problemDbContext;
            _classificationDbContext = classificationDbContext;
        }

        public async Task<UserStatsDto> GetUserStatsAsync(Guid userId, CancellationToken cancellationToken)
        {
            var solvedProblemIds = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.Verdict == Verdict.Accepted)
                .Select(s => s.ProblemId)
                .Distinct()
                .ToListAsync(cancellationToken);

            var solvedProblems = await _problemDbContext.Problems
                .AsNoTracking()
                .Where(p => solvedProblemIds.Contains(p.Id.Value))
                .Select(p => new { p.Difficulty })
                .ToListAsync(cancellationToken);

            var totalByDifficulty = await _problemDbContext.Problems
                .AsNoTracking()
                .Where(p => p.Status == ProblemStatus.Published)
                .GroupBy(p => p.Difficulty)
                .Select(g => new { Difficulty = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var totalSubmissions = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .CountAsync(s => s.UserId == userId, cancellationToken);

            var acceptedSubmissions = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .CountAsync(s => s.UserId == userId && s.Verdict == Verdict.Accepted, cancellationToken);

            var totalMap = totalByDifficulty.ToDictionary(x => x.Difficulty, x => x.Count);

            return new UserStatsDto
            {
                TotalSolved = solvedProblemIds.Count,
                EasySolved = solvedProblems.Count(p => p.Difficulty == Difficulty.Easy),
                MediumSolved = solvedProblems.Count(p => p.Difficulty == Difficulty.Medium),
                HardSolved = solvedProblems.Count(p => p.Difficulty == Difficulty.Hard),
                EasyTotal = totalMap.GetValueOrDefault(Difficulty.Easy, 0),
                MediumTotal = totalMap.GetValueOrDefault(Difficulty.Medium, 0),
                HardTotal = totalMap.GetValueOrDefault(Difficulty.Hard, 0),
                TotalSubmissions = totalSubmissions,
                AcceptedSubmissions = acceptedSubmissions
            };
        }

        public async Task<IReadOnlyList<UserHeatmapItemDto>> GetUserHeatmapAsync(Guid userId, CancellationToken cancellationToken)
        {
            var oneYearAgo = DateTimeOffset.UtcNow.AddYears(-1);

            var submissions = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.CreatedAt >= oneYearAgo)
                .Select(s => new { s.CreatedAt })
                .ToListAsync(cancellationToken);

            return submissions
                .GroupBy(s => s.CreatedAt.Date)
                .Select(g => new UserHeatmapItemDto
                {
                    Date = DateOnly.FromDateTime(g.Key),
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

        public async Task<IReadOnlyList<UserRecentAcDto>> GetUserRecentAcAsync(Guid userId, int count, CancellationToken cancellationToken)
        {
            var recentAc = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.Verdict == Verdict.Accepted)
                .GroupBy(s => s.ProblemId)
                .Select(g => new
                {
                    ProblemId = g.Key,
                    LastAcAt = g.Max(s => s.CreatedAt)
                })
                .OrderByDescending(x => x.LastAcAt)
                .Take(count)
                .ToListAsync(cancellationToken);

            if (!recentAc.Any()) return [];

            var problemIds = recentAc.Select(x => x.ProblemId).ToList();

            var problems = await _problemDbContext.Problems
                .AsNoTracking()
                .Where(p => problemIds.Contains(p.Id.Value))
                .Select(p => new { p.Id, p.Code, p.Title, p.Difficulty })
                .ToListAsync(cancellationToken);

            var problemMap = problems.ToDictionary(p => p.Id.Value);

            return recentAc
                .Where(x => problemMap.ContainsKey(x.ProblemId))
                .Select(x => new UserRecentAcDto
                {
                    ProblemId = x.ProblemId,
                    Code = problemMap[x.ProblemId].Code,
                    Title = problemMap[x.ProblemId].Title,
                    Difficulty = problemMap[x.ProblemId].Difficulty,
                    LastAcAt = x.LastAcAt
                })
                .ToList();
        }

        public async Task<IReadOnlyList<UserLanguageStatsDto>> GetUserLanguagesAsync(Guid userId, CancellationToken cancellationToken)
        {
            var solved = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.Verdict == Verdict.Accepted)
                .Select(s => new { s.Language, s.ProblemId })
                .Distinct()
                .ToListAsync(cancellationToken);

            return solved
                .GroupBy(s => s.Language)
                .Select(g => new UserLanguageStatsDto
                {
                    Language = g.Key.Value,
                    ProblemsSolved = g.Select(x => x.ProblemId).Distinct().Count()
                })
                .OrderByDescending(x => x.ProblemsSolved)
                .ToList();
        }

        public async Task<UserSkillsDto> GetUserSkillsAsync(Guid userId, CancellationToken cancellationToken)
        {
            var solvedProblemIds = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.Verdict == Verdict.Accepted)
                .Select(s => s.ProblemId)
                .Distinct()
                .ToListAsync(cancellationToken);

            if (!solvedProblemIds.Any())
                return new UserSkillsDto();

            var classificationRefs = await _problemDbContext.Set<ProblemClassificationRef>()
                .AsNoTracking()
                .Where(r => solvedProblemIds.Contains(r.Id.Value))
                .Select(r => new { r.Id.Value, r.ClassificationId })
                .ToListAsync(cancellationToken);

            var classificationIds = classificationRefs
                .Select(r => r.ClassificationId)
                .Distinct()
                .ToList();

            var classifications = await _classificationDbContext.Classifications
                .AsNoTracking()
                .Where(c => c.IsActive && c.Type == ClassificationType.Tag)
                .ToListAsync(cancellationToken);

            var relevantClassifications = classifications
                .Where(c => classificationIds.Contains(c.Id.Value))
                .ToList();

            var countPerClassification = classificationRefs
                .GroupBy(r => r.ClassificationId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.Value).Distinct().Count());

            var skills = relevantClassifications
                .Select(c => new UserSkillItemDto
                {
                    ClassificationId = c.Id.Value,
                    Name = c.Name,
                    ProblemsSolved = countPerClassification.GetValueOrDefault(c.Id.Value, 0)
                })
                .ToList();

            return new UserSkillsDto
            {
                Advanced = skills
                    .Where(s => s.ProblemsSolved >= 5)
                    .OrderByDescending(s => s.ProblemsSolved)
                    .ToList(),
                Intermediate = skills
                    .Where(s => s.ProblemsSolved is >= 2 and < 5)
                    .OrderByDescending(s => s.ProblemsSolved)
                    .ToList(),
                Fundamental = skills
                    .Where(s => s.ProblemsSolved == 1)
                    .ToList()
            };
        }

        public async Task<PagedResult<UserPracticeHistoryItemDto>> GetUserPracticeHistoryAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var latestSubmissions = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .GroupBy(s => s.ProblemId)
                .Select(g => new
                {
                    ProblemId = g.Key,
                    LastSubmittedAt = g.Max(s => s.CreatedAt),
                    LastVerdict = g.OrderByDescending(s => s.CreatedAt).First().Verdict,
                    SubmissionCount = g.Count()
                })
                .OrderByDescending(x => x.LastSubmittedAt)
                .ToListAsync(cancellationToken);

            var totalCount = latestSubmissions.Count;

            var paged = latestSubmissions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!paged.Any())
                return new PagedResult<UserPracticeHistoryItemDto>([], totalCount, page, pageSize);

            var problemIds = paged.Select(x => x.ProblemId).ToList();

            var problems = await _problemDbContext.Problems
                .AsNoTracking()
                .Where(p => problemIds.Contains(p.Id.Value))
                .Select(p => new { p.Id, p.Code, p.Title, p.Difficulty })
                .ToListAsync(cancellationToken);

            var problemMap = problems.ToDictionary(p => p.Id.Value);

            var items = paged
                .Where(x => problemMap.ContainsKey(x.ProblemId))
                .Select(x => new UserPracticeHistoryItemDto
                {
                    ProblemId = x.ProblemId,
                    Code = problemMap[x.ProblemId].Code,
                    Title = problemMap[x.ProblemId].Title,
                    Difficulty = problemMap[x.ProblemId].Difficulty,
                    LastSubmittedAt = x.LastSubmittedAt,
                    LastVerdict = x.LastVerdict,
                    SubmissionCount = x.SubmissionCount
                })
                .ToList();

            return new PagedResult<UserPracticeHistoryItemDto>(items, totalCount, page, pageSize);
        }
    }
}
