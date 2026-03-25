using MediatR;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserPracticeHistory
{
    public sealed record GetUserPracticeHistoryQuery(
        string Username,
        int Page = 1,
        int PageSize = 20,
        PracticeStatusFilter? Status = null,
        Difficulty? Difficulty = null
    ) : IRequest<UserPracticeHistoryDto>;

    public enum PracticeStatusFilter
    {
        Solved,
        Attempted
    }
}