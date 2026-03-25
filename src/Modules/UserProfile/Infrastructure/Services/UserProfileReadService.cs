using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemClassification.Infrastructure.Persistence;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.Modules.UserProfile.Application.Queries.GetUserPracticeHistory;
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

            var problemIds = solvedProblemIds.Select(ProblemId.From).ToList();

            var solvedProblems = await _problemDbContext.Problems
                .AsNoTracking()
                .Where(p => problemIds.Contains(p.Id))
                .Select(p => new { p.Difficulty })
                .ToListAsync(cancellationToken);

            var totalByDifficulty = await _problemDbContext.Problems
                .AsNoTracking()
                .Where(p => p.Status == ProblemStatus.Published)
                .GroupBy(p => p.Difficulty)
                .Select(g => new { Difficulty = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var submissionStats = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Accepted = g.Count(s => s.Verdict == Verdict.Accepted)
                })
                .FirstOrDefaultAsync(cancellationToken);

            var totalSubmissions = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .CountAsync(s => s.UserId == userId, cancellationToken);


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
                AcceptedSubmissions = submissionStats?.Accepted ?? 0
            };
        }

        public async Task<IReadOnlyList<UserHeatmapItemDto>> GetUserHeatmapAsync(Guid userId, int year, CancellationToken cancellationToken)
        {
            var startOfYear = new DateTimeOffset(year, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var endOfYear = startOfYear.AddYears(1);

            var submissions = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.CreatedAt >= startOfYear && s.CreatedAt < endOfYear)
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
            var Ids = problemIds.Select(ProblemId.From).ToList();

            var problems = await _problemDbContext.Problems
                .AsNoTracking()
                .Where(p => Ids.Contains(p.Id))
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

            var problemIds = solvedProblemIds.Select(ProblemClassificationRefId.From).ToList();

            var classificationRefs = await _problemDbContext.Set<ProblemClassificationRef>()
                .AsNoTracking()
                .Where(r => problemIds.Contains(r.Id))
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

        public async Task<UserPracticeHistoryDto> GetUserPracticeHistoryAsync(
            Guid userId,
            int page,
            int pageSize,
            PracticeStatusFilter? status,
            Difficulty? difficulty,
            CancellationToken cancellationToken)
        {
            // ── 1. Lấy tất cả submissions của user ──────────────────────────────
            var allSubmissions = await _submissionsDbContext.Submissions
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .Select(s => new
                {
                    s.Id,
                    s.ProblemId,
                    s.CreatedAt,
                    s.Verdict,
                    s.Language,
                    s.JudgeSummary.MaxTimeMs,
                    s.JudgeSummary.MaxMemoryKb
                })
                .ToListAsync(cancellationToken);

            // ── 2. Group theo ProblemId ──────────────────────────────────────────
            var grouped = allSubmissions
                .GroupBy(s => s.ProblemId)
                .Select(g =>
                {
                    var ordered = g.OrderByDescending(s => s.CreatedAt).ToList();
                    return new
                    {
                        ProblemId = g.Key,
                        LastSubmittedAt = ordered.First().CreatedAt,
                        LastVerdict = ordered.First().Verdict,
                        SubmissionCount = g.Count(),
                        Submissions = ordered.Select(s => new UserSubmissionDetailDto
                        {
                            SubmissionId = s.Id.Value,
                            SubmittedAt = s.CreatedAt,
                            Verdict = s.Verdict,
                            Language = s.Language.Value,
                            RuntimeMs = s.MaxTimeMs,
                            MemoryMb = s.MaxMemoryKb
                        }).ToList()
                    };
                })
                .ToList();

            // ── 3. Lấy problem info từ problemDbContext ──────────────────────────
            var allPublishedProblems = await _problemDbContext.Problems
                .AsNoTracking()
                .Where(p => p.Status == ProblemStatus.Published)
                .Select(p => new { p.Id, p.Code, p.Title, p.Difficulty })
                .ToListAsync(cancellationToken);

            var problemMap = allPublishedProblems.ToDictionary(p => p.Id.Value);

            // ── 4. Join + filter ─────────────────────────────────────────────────
            var joined = grouped
                .Where(x => problemMap.ContainsKey(x.ProblemId))
                .Select(x => new
                {
                    Problem = problemMap[x.ProblemId],
                    x.ProblemId,
                    x.LastSubmittedAt,
                    x.LastVerdict,
                    x.SubmissionCount,
                    x.Submissions
                })
                .ToList();

            // Filter theo Difficulty
            if (difficulty.HasValue)
                joined = joined.Where(x => x.Problem.Difficulty == difficulty.Value).ToList();

            // Filter theo Status
            if (status.HasValue)
            {
                joined = status.Value == PracticeStatusFilter.Solved
                    ? joined.Where(x => x.LastVerdict == Verdict.Accepted).ToList()
                    : joined.Where(x => x.LastVerdict != Verdict.Accepted).ToList();
            }

            // ── 5. Summary — tính trước khi phân trang ───────────────────────────
            var totalSubmissionsCount = joined.Sum(x => x.SubmissionCount);
            var acceptedSubmissionsCount = allSubmissions.Count(s =>
                problemMap.ContainsKey(s.ProblemId) && s.Verdict == Verdict.Accepted);

            var solvedProblems = joined.Where(x => x.LastVerdict == Verdict.Accepted).ToList();

            var summary = new UserPracticeHistorySummaryDto
            {
                TotalProblems = solvedProblems.Count,
                EasySolved = solvedProblems.Count(x => x.Problem.Difficulty == Difficulty.Easy),
                MediumSolved = solvedProblems.Count(x => x.Problem.Difficulty == Difficulty.Medium),
                HardSolved = solvedProblems.Count(x => x.Problem.Difficulty == Difficulty.Hard),
                TotalSubmissions = totalSubmissionsCount,
                AcceptanceRate = totalSubmissionsCount == 0
                    ? 0
                    : Math.Round((double)acceptedSubmissionsCount / totalSubmissionsCount * 100, 1)
            };

            // ── 6. Phân trang ────────────────────────────────────────────────────
            var totalCount = joined.Count;

            var items = joined
                .OrderByDescending(x => x.LastSubmittedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new UserPracticeHistoryItemDto
                {
                    ProblemId = x.ProblemId,
                    Code = x.Problem.Code,
                    Title = x.Problem.Title,
                    Difficulty = x.Problem.Difficulty,
                    LastSubmittedAt = x.LastSubmittedAt,
                    LastVerdict = x.LastVerdict,
                    SubmissionCount = x.SubmissionCount,
                    Submissions = x.Submissions
                })
                .ToList();

            return new UserPracticeHistoryDto
            {
                History = new PagedResult<UserPracticeHistoryItemDto>(items, totalCount, page, pageSize),
                Summary = summary
            };
        }
    }
}
