using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Queries.GetMyProfile
{
    public sealed class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, MyProfileDto>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyProfileQueryHandler(IUserReadRepository userReadRepository, ICurrentUserService currentUserService)
        {
            _userReadRepository = userReadRepository;
            _currentUserService = currentUserService;
        }

        public async Task<MyProfileDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetMyProfileAsync(_currentUserService.UserId, cancellationToken);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            return user;
        }
    }
}