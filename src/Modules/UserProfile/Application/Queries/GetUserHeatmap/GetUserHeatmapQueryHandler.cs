using MediatR;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.Modules.UserProfile.Application.Exceptions;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserHeatmap
{
    public sealed class GetUserHeatmapQueryHandler : IRequestHandler<GetUserHeatmapQuery, UserHeatmapDto>
    {
        private readonly IUserIdentityReadService _userIdentityReadService;
        private readonly IUserProfileReadService _userProfileReadService;

        public GetUserHeatmapQueryHandler(IUserIdentityReadService userIdentityReadService, IUserProfileReadService userProfileReadService)
        {
            _userIdentityReadService = userIdentityReadService;
            _userProfileReadService = userProfileReadService;
        }

        public async Task<UserHeatmapDto> Handle(GetUserHeatmapQuery request, CancellationToken cancellationToken)
        {
            var user = await _userIdentityReadService.GetUserByUsernameAsync(request.Username, cancellationToken)
                ?? throw new UserProfileNotFoundException(request.Username);

            var currentYear = DateTimeOffset.UtcNow.Year;
            var registerdYear = user.CreatedAt.Year;
            var selectedYear = request.Year ?? currentYear;

            if (selectedYear < registerdYear || selectedYear > currentYear)
                throw new InvalidOperationException("Invalid year.");

            var availableYears = Enumerable
                .Range(registerdYear, currentYear - registerdYear + 1)
                .Reverse()
                .ToList();

            var items = await _userProfileReadService.GetUserHeatmapAsync(user.Id, selectedYear, cancellationToken);

            var totalSubmissions = items.Sum(x => x.Count);
            var totalActiveDays = items.Count;

            return new UserHeatmapDto
            {
                Items = items,
                AvailableYears = availableYears,
                SelectedYear = selectedYear,
                TotalSubmissions = totalSubmissions,
                TotalActiveDays = totalActiveDays
            };
        }
    }
}